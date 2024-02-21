using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Web.Models.Tables.EnumTables
{
    public enum StatusEnum
    {
        Free = 1,
        Reserved,
        Sold
    }

    public class Status : EnumTable
    {
        [NotMapped]
        public static Type Enum { get; set; } = typeof(StatusEnum);
    }
}
