using Cinema.Data.Models.Tables;
using System.Linq.Expressions;

namespace Cinema.Data.Models.DTOs
{
    public class HallIndex
    {
        public String Name { get; set; } = null!;
        public Int32 RowCount { get; set; }
        public Int32 ColumnCount { get; set; }

        public static Expression<Func<Hall, HallIndex>> Projection { get; }
            = hall => new HallIndex
            {
                Name = hall.Name,
                RowCount = hall.RowCount,
                ColumnCount = hall.ColumnCount
            };

        public static HallIndex Create(Hall hall) => Projection.Compile().Invoke(hall);
    }
}
