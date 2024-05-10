using Cinema.Data.Models.DTOs;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models.Tables
{
    public class Movie : Table
    {
        [DisplayName("Cím")]
        public String Title { get; set; } = null!;

        [DisplayName("Rendező")]
        public String Director { get; set; } = null!;

        [DataType(DataType.MultilineText)]
        [DisplayName("Szinposzis")]
        public String Synopsis { get; set; } = null!;

        [DisplayName("Hossz (perc)")]
        public Int32 Length { get; set; }

        [DisplayName("Plakát")]
        public Byte[]? Image { get; set; }

        [DisplayName("Felviteli dátum")]
        public DateTime Entry { get; set; }

        [DisplayName("Előadások")]
        public virtual ICollection<Show> Shows { get; set; } = [];

        public Movie() { }

        public Movie(MovieDTO movie) 
        {
            Title = movie.Title ?? "";
            Director = movie.Director ?? "";
            Synopsis = movie.Synopsis ?? "";
            Length = movie.Length ?? 0;
            Image = movie.Image;
            Entry = movie.Entry ?? new DateTime();
        }
    }
}
