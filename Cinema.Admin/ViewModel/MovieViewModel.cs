using Cinema.Data.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Cinema.Admin.ViewModel
{
    public class MovieViewModel : ViewModelBase, IEditableObject
    {
        private Int32? _id;
        private String? _title;
        private String? _director;
        private String? _synopsis;
        private Int32? _length;
        private Byte[]? _image;
        private DateTime? _entry;

        private Boolean _isDirty = false;
        private MovieViewModel _backup = null!;

        public Int32? Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }
        public String? Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }
        public String? Director
        {
            get => _director;
            set { _director = value; OnPropertyChanged(); }
        }
        public String? Synopsis
        {
            get => _synopsis;
            set { _synopsis = value; OnPropertyChanged(); }
        }
        public Int32? Length
        {
            get => _length;
            set { _length = value; OnPropertyChanged(); }
        }
        public Byte[]? Image
        {
            get => _image;
            set { _image = value; OnPropertyChanged(); }
        }
        public DateTime? Entry
        {
            get => _entry;
            set { _entry = value; OnPropertyChanged(); }
        }

        public event EventHandler EditEnded;

        public void BeginEdit()
        {
            if (!_isDirty)
            {
                _backup = (MovieViewModel)MemberwiseClone();
                _isDirty = true;
            }
        }

        public void CancelEdit()
        {
            if (_isDirty)
            {
                Id = _backup.Id;
                Title = _backup.Title;
                Director = _backup.Director;
                Synopsis = _backup.Synopsis;
                Length = _backup.Length;
                Image = _backup.Image;
                Entry = _backup.Entry;
                _isDirty = false;
                _backup = null!;
            }
        }

        public void EndEdit()
        {
            if (_isDirty)
            {
                EditEnded?.Invoke(this, EventArgs.Empty);
                _isDirty = false;
                _backup = null!;
            }
        }

        public static explicit operator MovieViewModel(MovieDTO dto) => new MovieViewModel
        {
            Id = dto.Id,
            Title = dto.Title,
            Director = dto.Director,
            Synopsis = dto.Synopsis,
            Length = dto.Length,
            Image = dto.Image,
            Entry = dto.Entry
        };

        public static explicit operator MovieDTO(MovieViewModel vm) => new MovieDTO
        {
            Id = vm.Id,
            Title = vm.Title,
            Director = vm.Director,
            Synopsis = vm.Synopsis,
            Length = vm.Length,
            Image = vm.Image,
            Entry = vm.Entry
        };
    }

    public class ListValidationRule : ValidationRule
    {
        public override ValidationResult Validate(Object value, CultureInfo cultureInfo)
        {
            MovieViewModel movie = ((value as BindingGroup)!.Items[0] as MovieViewModel)!;

            if (movie.Title.IsNullOrEmpty())
                return new ValidationResult(false, "Cím megadása kötelező!");
            else if (movie.Director.IsNullOrEmpty())
                return new ValidationResult(false, "Rendező megadása kötelező!");
            else if (movie.Synopsis.IsNullOrEmpty())
                return new ValidationResult(false, "Szinopszis megadása kötelező!");
            else if (movie.Length == null)
                return new ValidationResult(false, "Hossz megadása kötelező!");
            else
                return ValidationResult.ValidResult;
        }
    }
}
