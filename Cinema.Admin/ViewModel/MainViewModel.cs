using Cinema.Admin.Model;
using Cinema.Data.Models.DTOs;
using Cinema.Data.Models.Tables.Enums;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace Cinema.Admin.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly CinemaAPIService _service = null!;
        private ObservableCollection<HallViewModel> _halls = null!;
        private ObservableCollection<MovieViewModel> _movies = null!;
        private ObservableCollection<SeatViewModel> _seats = null!;
        private ObservableCollection<ShowViewModel> _shows = null!;
        private MovieViewModel _selectedMovie = null!;
        private SeatViewModel _selectedSeat = null!;
        private ShowViewModel _selectedShow = null!;

        public ObservableCollection<HallViewModel> Halls
        {
            get { return _halls; }
            set { _halls = value; OnPropertyChanged(); }
        }

        public List<HallViewModel> HallsForCombo { get => [.. Halls]; }

        public ObservableCollection<MovieViewModel> Movies
        {
            get { return _movies; }
            set { _movies = value; OnPropertyChanged(); }
        }

        public List<MovieViewModel> MoviesForCombo { get => [.. Movies]; }

        public ObservableCollection<SeatViewModel> Seats
        {
            get
            {
                return new ObservableCollection<SeatViewModel>(
                _seats.Where(s => (_selectedShow ?? _shows[0]).SeatIds.Contains(s.Id)));
            }
            set { _seats = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ShowViewModel> Shows
        {
            get { return _shows; }
            set { _shows = value; OnPropertyChanged(); }
        }

        public MovieViewModel SelectedMovie
        {
            get { return _selectedMovie; }
            set { _selectedMovie = value; OnPropertyChanged(); }
        }

        public SeatViewModel SelectedSeat
        {
            get { return _selectedSeat; }
            set { _selectedSeat = value; OnPropertyChanged(); }
        }

        public ShowViewModel SelectedShow
        {
            get { return _selectedShow; }
            set
            {
                _selectedShow = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Seats));
            }
        }

        #region Commands

        public DelegateCommand LogoutCommand { get; private set; }

        #region Movie commands

        public DelegateCommand RefreshMoviesCommand { get; private set; }
        public DelegateCommand AddMovieCommand { get; private set; }
        public DelegateCommand EditMovieCommand { get; private set; }
        public DelegateCommand DeleteMovieCommand { get; private set; }

        public DelegateCommand SaveEditMovieCommand { get; private set; }
        public DelegateCommand CancelEditMovieCommand { get; private set; }
        public DelegateCommand ChangeMovieImageCommand { get; private set; }

        #endregion

        #region Show commands

        public DelegateCommand RefreshShowsCommand { get; private set; }
        public DelegateCommand AddShowCommand { get; private set; }
        public DelegateCommand EditShowCommand { get; private set; }
        public DelegateCommand DeleteShowCommand { get; private set; }

        public DelegateCommand SaveEditShowCommand { get; private set; }
        public DelegateCommand CancelEditShowCommand { get; private set; }

        #endregion

        #region Seat commands

        public DelegateCommand SellSeatCommand { get; private set; }

        #endregion

        #endregion

        #region Events

        public event EventHandler LogoutSucceeded = null!;

        public event EventHandler StartingMovieEdit = null!;
        public event EventHandler FinishingMovieEdit = null!;
        public event EventHandler StartingMovieImageChange = null!;

        public event EventHandler StartingShowEdit = null!;
        public event EventHandler FinishingShowEdit = null!;

        #endregion

        public MainViewModel(CinemaAPIService service)
        {
            _service = service;

            LogoutCommand = new DelegateCommand(async _ => await LogoutAsync());

            Task.Run(LoadHallsAsync).Wait();
            Task.Run(LoadMoviesAsync).Wait();
            Task.Run(LoadShowsAsync).Wait();
            Task.Run(LoadSeatsAsync).Wait();

            RefreshMoviesCommand = new DelegateCommand(async _ => await LoadMoviesAsync());
            AddMovieCommand = new DelegateCommand(_ =>
                !(SelectedMovie == null) && SelectedMovie.Id != 0,
                _ => AddMovie());
            EditMovieCommand = new DelegateCommand(_ => SelectedMovie != null,
                _ => StartEditMovie());
            DeleteMovieCommand = new DelegateCommand(_ => SelectedMovie != null,
                async _ => await DeleteMovieAsync(SelectedMovie));
            SaveEditMovieCommand = new DelegateCommand(_ =>
                String.IsNullOrEmpty(SelectedMovie?[nameof(MovieViewModel.Title)]),
                async _ => await SaveMovieEditAsync());
            CancelEditMovieCommand = new DelegateCommand(_ => CancelMovieEdit());
            ChangeMovieImageCommand = new DelegateCommand(_ =>
                StartingMovieImageChange?.Invoke(this, EventArgs.Empty));

            RefreshShowsCommand = new DelegateCommand(async _ => await LoadShowsAsync());
            AddShowCommand = new DelegateCommand(_ =>
                !(SelectedMovie == null) && SelectedShow.Id != 0,
                _ => AddShow());
            EditShowCommand = new DelegateCommand(_ => SelectedShow != null,
                _ => StartEditShow());
            DeleteShowCommand = new DelegateCommand(_ => SelectedShow != null,
                async _ => await DeleteShowAsync(SelectedShow));
            SaveEditShowCommand = new DelegateCommand(async _ => await SaveShowEditAsync());
            CancelEditShowCommand = new DelegateCommand(_ => CancelShowEdit());

            SellSeatCommand = new DelegateCommand(_ => SelectedShow != null,
                async _ => await SellSeatAsync());
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

        #region Halls

        private async Task LoadHallsAsync()
        {
            try
            {
                Halls = new ObservableCollection<HallViewModel>((await _service.LoadHallsAsync())
                    .Select(m => (HallViewModel)m));
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
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

        private async Task DeleteMovieAsync(MovieViewModel item)
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

        private async Task SaveMovieEditAsync()
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

        #region Seats

        private async Task LoadSeatsAsync()
        {
            try
            {
                Seats = new ObservableCollection<SeatViewModel>((await _service.LoadSeatsAsync())
                    .Select(m => (SeatViewModel)m));
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
        }

        private async Task SellSeatAsync()
        {
            try
            {
                await _service.SellSeatAsync((SeatDTO)SelectedSeat);
                SelectedSeat.Status = Status.Sold;
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
        }

        #endregion

        #region Shows

        private async Task LoadShowsAsync()
        {
            try
            {
                Shows = new ObservableCollection<ShowViewModel>((await _service.LoadShowsAsync())
                    .Select(m => (ShowViewModel)m));
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
        }

        private void AddShow()
        {
            var newShow = new ShowViewModel
            {
                Start = DateTime.UtcNow,
                Movie = Movies.First(),
                Hall = Halls.First(),
                SeatIds = []
            };
            Shows.Add(newShow);
            SelectedShow = newShow;
            StartEditShow();
        }

        private void StartEditShow()
        {
            SelectedShow.BeginEdit();
            StartingShowEdit?.Invoke(this, EventArgs.Empty);
        }

        private async Task DeleteShowAsync(ShowViewModel item)
        {
            try
            {
                await _service.DeleteShowAsync(item.Id);
                Shows.Remove(SelectedShow);
                SelectedShow = null!;
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
        }

        private void CancelShowEdit()
        {
            if (SelectedShow is null || !SelectedShow.IsDirty)
                return;

            if (SelectedShow.Id == 0)
            {
                Shows.Remove(SelectedShow);
                SelectedShow = null!;
            }
            else
            {
                SelectedShow.CancelEdit();
            }
            FinishingShowEdit?.Invoke(this, EventArgs.Empty);
        }

        private async Task SaveShowEditAsync()
        {
            try
            {
                if (SelectedShow.Id == 0)
                {
                    var itemDto = (ShowDTO)SelectedShow;
                    await _service.CreateShowAsync(itemDto);
                    SelectedShow.Id = itemDto.Id;
                    SelectedShow.EndEdit();
                }
                else
                {
                    await _service.UpdateShowAsync((ShowDTO)SelectedShow);
                    SelectedShow.EndEdit();
                }
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
            FinishingShowEdit?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
