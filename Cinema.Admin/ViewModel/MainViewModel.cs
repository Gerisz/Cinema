using Cinema.Admin.Model;
using Cinema.Data.Models.DTOs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Windows.Controls;

namespace Cinema.Admin.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly CinemaAPIService _service;
        private ObservableCollection<MovieViewModel> _movies;
        private MovieViewModel _selectedMovie;

        public ObservableCollection<MovieViewModel> Movies
        {
            get { return _movies; }
            set { _movies = value; OnPropertyChanged(); }
        }

        public MovieViewModel SelectedMovie
        {
            get { return _selectedMovie; }
            set { _selectedMovie = value; OnPropertyChanged(); }
        }

        #region Commands

        public DelegateCommand LogoutCommand { get; private set; }
        public DelegateCommand TabChangedCommand { get; private set; }

        #region Movie commands

        public DelegateCommand RefreshMoviesCommand { get; private set; }
        public DelegateCommand AddMovieCommand { get; private set; }
        public DelegateCommand EditMovieCommand { get; private set; }
        public DelegateCommand DeleteMovieCommand { get; private set; }

        public DelegateCommand SaveEditMovieCommand { get; private set; }
        public DelegateCommand CancelEditMovieCommand { get; private set; }
        public DelegateCommand ChangeMovieImageCommand { get; private set; }

        #endregion

        #endregion

        #region Events

        public event EventHandler LogoutSucceeded;
        public event EventHandler StartingMovieEdit;
        public event EventHandler FinishingMovieEdit;
        public event EventHandler StartingMovieImageChange;

        #endregion

        public MainViewModel(CinemaAPIService service)
        {
            _service = service;

            LogoutCommand = new DelegateCommand(async _ => await LogoutAsync());
            Task.Run(LoadMoviesAsync).Wait();

            RefreshMoviesCommand = new DelegateCommand(async _ => await LoadMoviesAsync());
            AddMovieCommand = new DelegateCommand(_ =>
                !(SelectedMovie == null) && SelectedMovie.Id != 0,
                _ => AddMovie());
            EditMovieCommand = new DelegateCommand(_ => SelectedMovie != null,
                _ => StartEditMovie());
            DeleteMovieCommand = new DelegateCommand(_ => SelectedMovie != null,
                _ => DeleteMovie(SelectedMovie));

            SaveEditMovieCommand = new DelegateCommand(_ =>
                String.IsNullOrEmpty(SelectedMovie?[nameof(MovieViewModel.Title)]),
                _ => SaveMovieEdit());
            CancelEditMovieCommand = new DelegateCommand(_ => CancelMovieEdit());
            ChangeMovieImageCommand = new DelegateCommand(_ =>
                StartingMovieImageChange?.Invoke(this, EventArgs.Empty));
        }

        #region Authentication

        private async Task LogoutAsync()
        {
            try
            {
                await _service.LogoutAsync();
                OnLogoutSuccess();
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
        }

        private void OnLogoutSuccess()
        {
            LogoutSucceeded?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Movies

        private async Task LoadMoviesAsync()
        {
            try
            {
                Movies = new ObservableCollection<MovieViewModel>((await _service.LoadMoviesAsync())
                    .Select(m => (MovieViewModel)m));
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
        }

        private void AddMovie()
        {
            var newMovie = new MovieViewModel
            {
                Title = "Új film",
                Director = "Rendező",
                Synopsis = "Szinopszis",
                Length = 0,
                Entry = DateTime.UtcNow
            };
            Movies.Add(newMovie);
            SelectedMovie = newMovie;
            StartEditMovie();
        }

        private void StartEditMovie()
        {
            SelectedMovie.BeginEdit();
            StartingMovieEdit?.Invoke(this, EventArgs.Empty);
        }

        private async void DeleteMovie(MovieViewModel item)
        {
            try
            {
                await _service.DeleteMovieAsync(item.Id);
                Movies.Remove(SelectedMovie);
                SelectedMovie = null!;
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
        }

        private void CancelMovieEdit()
        {
            if (SelectedMovie is null || !SelectedMovie.IsDirty)
                return;

            if (SelectedMovie.Id == 0)
            {
                Movies.Remove(SelectedMovie);
                SelectedMovie = null!;
            }
            else
            {
                SelectedMovie.CancelEdit();
            }
            FinishingMovieEdit?.Invoke(this, EventArgs.Empty);
        }

        private async void SaveMovieEdit()
        {
            try
            {
                if (SelectedMovie.Id == 0)
                {
                    var itemDto = (MovieDTO)SelectedMovie;
                    await _service.CreateMovieAsync(itemDto);
                    SelectedMovie.Id = itemDto.Id;
                    SelectedMovie.EndEdit();
                }
                else
                {
                    await _service.UpdateMovieAsync((MovieDTO)SelectedMovie);
                    SelectedMovie.EndEdit();
                }
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
            FinishingMovieEdit?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
