using Cinema.Data.Models.DTOs;

namespace Cinema.Admin.ViewModel
{
    public class HallViewModel : ViewModelBase
    {
        private Int32 _id;
        private String _name = null!;
        private Int32 _rowCount;
        private Int32 _columnCount;

        private HallViewModel _backup = null!;

        public Int32 Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public String Name
        {
            get => _name; set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public Int32 RowCount
        {
            get => _rowCount; set
            {
                _rowCount = value;
                OnPropertyChanged();
            }
        }

        public Int32 ColumnCount
        {
            get => _columnCount; set
            {
                _columnCount = value;
                OnPropertyChanged();
            }
        }

        public Boolean IsDirty { get; private set; } = false;

        public String Error => String.Empty;

        public void BeginEdit()
        {
            if (!IsDirty)
            {
                _backup = (HallViewModel)MemberwiseClone();
                IsDirty = true;
            }
        }

        public void CancelEdit()
        {
            if (IsDirty)
            {
                Id = _backup.Id;
                Name = _backup.Name;
                RowCount = _backup.RowCount;
                ColumnCount = _backup.ColumnCount;

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

        public static explicit operator HallViewModel(HallDTO dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            RowCount = dto.RowCount,
            ColumnCount = dto.ColumnCount
        };

        public static explicit operator HallDTO(HallViewModel vm) => new()
        {
            Id = vm.Id,
            Name = vm.Name,
            RowCount = vm.RowCount,
            ColumnCount = vm.ColumnCount
        };

        public override String ToString()
        {
            return $"{Name} - ({RowCount}x{ColumnCount} ülés)";
        }
    }
}
