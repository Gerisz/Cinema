using Cinema.Data.Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models.Tables
{
    public class Movie : Table
    {
        public String Title { get; set; } = null!;
        public String Director { get; set; } = null!;

        [DataType(DataType.MultilineText)]
        public String Synopsis { get; set; } = null!;

        public Int32 Length { get; set; }

        public Byte[]? Image { get; set; }

        public DateTime Entry { get; set; }

        public virtual ICollection<Show> Shows { get; set; } = [];

        public Movie() { }

        public Movie(MovieCreate movie) 
        {
            Title = movie.Title;
            Director = movie.Director;
            Synopsis = movie.Synopsis;
            Length = movie.Length;
            Image = movie.Image;
            Entry = movie.Entry;
        }
    }
}
