using Cinema.Data.Models.DTOs;

namespace Cinema.Admin.ViewModel
{
    public class ShowViewModel : ViewModelBase
    {
        private Int32 _id;
        private DateTime _start;
        private MovieViewModel _movie = null!;
        private HallViewModel _hall = null!;
        private IEnumerable<Int32> _seatIds = [];

        private ShowViewModel _backup = null!;

        public String this[String columnName]
        {
            get
            {
                String error = String.Empty;
                return error;
            }
        }

        public Int32 Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public DateTime Start
        {
            get => _start;
            set
            {
                _start = value;
                OnPropertyChanged();
            }
        }

        public MovieViewModel Movie
        {
            get => _movie;
            set
            {
                _movie = value;
                OnPropertyChanged();
            }
        }

        public HallViewModel Hall
        {
            get => _hall; set
            {
                _hall = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Int32> SeatIds
        {
            get => _seatIds; set
            {
                _seatIds = value;
                OnPropertyChanged();
            }
        }

        public Boolean IsDirty { get; private set; } = false;

        public String Error => String.Empty;

        public void BeginEdit()
        {
            if (!IsDirty)
            {
                _backup = (ShowViewModel)this.MemberwiseClone();
                IsDirty = true;
            }
        }

        public void CancelEdit()
        {
            if (IsDirty)
            {
                Id = _backup.Id;
                Start = _backup.Start;
                Movie = _backup.Movie;
                Hall = _backup.Hall;
                SeatIds = _backup.SeatIds;

                IsDirty = false;
                _backup = null!;
            }
        }

        public void EndEdit()
        {
            if (IsDirty)
            {
                IsDirty = false;
                _backup = null!;
            }
        }

        public static explicit operator ShowViewModel(ShowDTO dto) => new()
        {
            Id = dto.Id,
            Start = dto.Start,
            Movie = (MovieViewModel)dto.Movie,
            Hall = (HallViewModel)dto.Hall,
            SeatIds = dto.SeatIds
        };

        public static explicit operator ShowDTO(ShowViewModel vm) => new()
        {
            Id = vm.Id,
            Start = vm.Start,
            Movie = (MovieDTO)vm.Movie,
            Hall = (HallDTO)vm.Hall,
            SeatIds = vm.SeatIds
        };

        public override String ToString()
        {
            return $"{Movie.Title}: {Start} - {Start + new TimeSpan(0, Movie.Length, 0)}";
        }
    }
}
