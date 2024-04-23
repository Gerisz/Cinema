using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models.DTOs
{
    public class MovieCreate
    {
        public String Title { get; set; } = null!;
        public String Director { get; set; } = null!;

        [DataType(DataType.MultilineText)]
        public String Synopsis { get; set; } = null!;

        public Int32 Length { get; set; }

        public Byte[]? Image { get; set; }

        public DateTime Entry { get; set; }
    }
}
