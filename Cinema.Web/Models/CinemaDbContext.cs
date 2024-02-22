using Cinema.Web.Models.Tables;
using Cinema.Web.Models.Tables.EnumTables;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using System.Diagnostics.CodeAnalysis;
//using System.Reflection;

namespace Cinema.Web.Models
{
    public class CinemaDbContext : IdentityDbContext<Employee>
    {
        public DbSet<Hall> Halls { get; set; } = null!;
        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Seat> Seats { get; set; } = null!;
        public DbSet<Show> Shows { get; set; } = null!;
        public DbSet<Status> Statuses { get; set; } = null!;

        public CinemaDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            BuildEnumTable<StatusEnum, Status>(builder);

            /*Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsSubclassOf(typeof(EnumTable))).ToList()
                .ForEach(t => GetType().GetMethods().Single(m => m.Name == "BuildEnumTable")
                    .MakeGenericMethod((Type)t.GetProperties().Single(p => p.Name == "EnumTable").GetValue(null)!, t)
                    .Invoke(null, new[] { builder }));*/
        }

        /*[SuppressMessage("CodeQuality",
            "IDE0051:Remove unused private members",
            Justification = "It isn't used, but invoked via Assembly")]*/
        private static void BuildEnumTable<TEnum, T>(ModelBuilder builder)
            where TEnum : struct, Enum
            where T : EnumTable, new()
        {
            builder
                .Entity<T>()
                .Property(e => e.Id)
                .HasConversion<int>();
            builder
                .Entity<T>().HasData(
                    Enum.GetValues<TEnum>()
                        .Select(e => new T()
                        {
                            Id = Convert.ToInt32(e),
                            Value = e.ToString() ?? ""
                        }
                    )
            );
        }
    }
}
