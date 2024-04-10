﻿using Cinema.Data.Models.Tables;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Cinema.Web.Models
{
    public class SeatReserve
    {
        public int ShowId { get; set; }
        public List<SeatIndex> Seats { get; set; } = [];
        public (int RowCount, int ColumnCount) HallSize { get; set; }

        [DisplayName("Név")]
        [Required(ErrorMessage = "Név megadása kötelező!")]
        public string Name { get; set; } = null!;

        [DataType(DataType.PhoneNumber)]
        [DisplayName("Telefonszám")]
        [Required(ErrorMessage = "Telefonszám megadása kötelező!")]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        public static Expression<Func<Show, SeatReserve>> Projection { get; }
            = show => new SeatReserve()
            {
                ShowId = show.Id,
                Seats = show.Seats
                    .AsQueryable()
                    .OrderBy(s => s.Row)
                    .ThenBy(s => s.Column)
                    .Select(SeatIndex.Projection)
                    .ToList(),
                HallSize = new(show.Hall.RowCount, show.Hall.ColumnCount)
            };

        public static SeatReserve Create(Show show) => Projection.Compile().Invoke(show);
    }
}
