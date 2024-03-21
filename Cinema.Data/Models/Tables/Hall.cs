namespace Cinema.Data.Models.Tables
{
    public class Hall : Table
    {
        public String Name { get; set; } = null!;
        public Int32 RowCount { get; set; }
        public Int32 ColumnCount { get; set; }

        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
