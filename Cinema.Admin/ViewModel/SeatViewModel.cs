using Cinema.Data.Models.DTOs;
using Cinema.Data.Models.Tables.Enums;

namespace Cinema.Admin.ViewModel
{
    public class SeatViewModel : ViewModelBase
    {
        private Int32 _id;
        private Int32 _row;
        private Int32 _column;
        private Status _status;
        private String _reservantName = "";
        private String _reservantPhoneNumber = "";

        public Int32 Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public Int32 Row
        {
            get => _row;
            set
            {
                _row = value;
                OnPropertyChanged();
            }
        }

        public Int32 Column
        {
            get => _column;
            set
            {
                _column = value;
                OnPropertyChanged();
            }
        }

        public Status Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public String ReservantName
        {
            get => _reservantName;
            set
            {
                _reservantName = value;
                OnPropertyChanged();
            }
        }

        public String ReservantPhoneNumber
        {
            get => _reservantPhoneNumber;
            set
            {
                _reservantPhoneNumber = value;
                OnPropertyChanged();
            }
        }

        public static explicit operator SeatViewModel(SeatDTO dto) => new()
        {
            Id = dto.Id,
            Row = dto.Row,
            Column = dto.Column,
            Status = dto.Status,
            ReservantName = dto.ReservantName ?? "",
            ReservantPhoneNumber = dto.ReservantPhoneNumber ?? "",
        };

        public static explicit operator SeatDTO(SeatViewModel vm) => new()
        {
            Id = vm.Id,
            Row = vm.Row,
            Column = vm.Column,
            Status = vm.Status,
            ReservantName = vm.ReservantName,
            ReservantPhoneNumber = vm.ReservantPhoneNumber,
        };
    }
}
