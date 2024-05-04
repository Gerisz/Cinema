using Cinema.Admin.Model;
using Cinema.Data.Models.DTOs;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Controls;

namespace Cinema.Admin.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly CinemaAPIService _service;
        private ObservableCollection<MovieViewModel> _movies;
        private MovieViewModel _movie;

        public ObservableCollection<MovieViewModel> Movies
        {
            get { return _movies; }
            set { _movies = value; OnPropertyChanged(); }
        }

        public MovieViewModel Movie
        {
            get { return _movie; }
            set { _movie = value; OnPropertyChanged(); }
        }

        public DelegateCommand NewMovieCommand { get; private set; }
        public DelegateCommand LogoutCommand { get; private set; }

        public event EventHandler LogoutSucceeded;

        public MainViewModel(CinemaAPIService service)
        {
            _service = service;

            LogoutCommand = new DelegateCommand(async _ => await LogoutAsync());
            NewMovieCommand = new DelegateCommand(async _ => await NewMovieAsync(Movie));
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

        private async void LoadMoviesAsync()
        {
            try
            {
                Movies = new ObservableCollection<MovieViewModel>
                    ((await _service.LoadMoviesAsync()).Select(list =>
                {
                    var movie = (MovieViewModel)list;
                    return movie;
                }));
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
        }

        private async Task NewMovieAsync(MovieViewModel newMovie)
        {
            try
            {
                var movie = _service.CreateMovieAsync((MovieDTO)newMovie);

                if (movie.Id != null)
                    _movies.Add((MovieViewModel)await movie);
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
        }

        #endregion
    }
}
