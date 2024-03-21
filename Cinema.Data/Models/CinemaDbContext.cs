using Cinema.Data.Models.Tables;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Data.Models
{
    public class CinemaDbContext : IdentityDbContext<Employee>
    {
        public DbSet<Hall> Halls { get; set; } = null!;
        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Seat> Seats { get; set; } = null!;
        public DbSet<Show> Shows { get; set; } = null!;

        public CinemaDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
