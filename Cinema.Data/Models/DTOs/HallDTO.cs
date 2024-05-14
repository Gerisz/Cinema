using Cinema.Data.Models.Tables;
using System.Linq.Expressions;

namespace Cinema.Data.Models.DTOs
{
    public class HallDTO
    {
        public Int32 Id { get; set; }
        public String Name { get; set; } = null!;
        public Int32 RowCount { get; set; }
        public Int32 ColumnCount { get; set; }

        public static Expression<Func<Hall, HallDTO>> Projection { get; }
            = hall => new HallDTO
            {
                Id = hall.Id,
                Name = hall.Name,
                RowCount = hall.RowCount,
                ColumnCount = hall.ColumnCount
            };

        public static HallDTO Create(Hall hall) => Projection.Compile().Invoke(hall);
    }
}
