using Cinema.Data.Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Admin.ViewModel
{
    public class MovieViewModel : ViewModelBase
    {
        private Int32 _id;
        private String _title = null!;
        private String _director = null!;
        private String _synopsis = null!;
        private Int32 _length;
        private Byte[]? _image;
        private DateTime _entry;

        private MovieViewModel _backup = null!;

        public String this[String columnName]
        {
            get
            {
                String error = String.Empty;
                switch (columnName)
                {
                    case nameof(Title):
                        if (String.IsNullOrEmpty(Title))
                            error = "Title cannot be empty.";
                        else if (Title.Length > 30)
                            error = "Title cannot be longer than 30 characters.";
                        break;
                }
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

        public String Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public String Director
        {
            get => _director;
            set
            {
                _director = value;
                OnPropertyChanged();
            }
        }

        public String Synopsis
        {
            get => _synopsis;
            set
            {
                _synopsis = value;
                OnPropertyChanged();
            }
        }

        public Int32 Length
        {
            get => _length;
            set
            {
                _length = value;
                OnPropertyChanged();
            }
        }

        public Byte[] Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }

        public DateTime Entry
        {
            get => _entry;
            set
            {
                _entry = value;
                OnPropertyChanged();
            }
        }

        public Boolean IsDirty { get; private set; } = false;

        public String Error => String.Empty;

        public void BeginEdit()
        {
            if (!IsDirty)
            {
                _backup = (MovieViewModel)this.MemberwiseClone();
                IsDirty = true;
            }
        }

        public void CancelEdit()
        {
            if (IsDirty)
            {
                Id = _backup.Id;
                Title = _backup.Title;
                Synopsis = _backup.Synopsis;
                Length = _backup.Length;
                Image = _backup.Image;
                Entry = _backup.Entry;

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

        public static explicit operator MovieViewModel(MovieDTO dto) => new()
        {
            Id = dto.Id,
            Title = dto.Title,
            Synopsis = dto.Synopsis,
            Length = dto.Length,
            Image = dto.Image ?? [],
            Entry = dto.Entry
        };

        public static explicit operator MovieDTO(MovieViewModel vm) => new()
        {
            Id = vm.Id,
            Title = vm.Title,
            Synopsis = vm.Synopsis,
            Length = vm.Length,
            Image = vm.Image,
            Entry = vm.Entry
        };
    }
}
