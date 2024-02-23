using Cinema.Web.Models.Tables;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Cinema.Web.Models
{

    public static class DbInitializer
    {
        private static readonly Random Random = new Random();

        public static async Task InitializeAsync(CinemaDbContext context/*, String imageDirectory*/)
        {
            await context.Database.MigrateAsync();

            if (context.Any())
                return;

            // await context.Statuses.ExecuteDeleteAsync();

            List<Movie> defaultMovies =
                JsonSerializer.Deserialize<List<Movie>>(
                    File.ReadAllText("Models\\DefaultValues\\movies.json")
                ) ?? [];

            List<Hall> defaultHalls =
                [
                    new Hall
                    {
                        Name = "Nagyterem",
                        RowCount = 10,
                        ColumnCount = 20
                    },
                    new Hall
                    {
                        Name = "Kisterem 1",
                        RowCount = 8,
                        ColumnCount = 10,
                    },
                    new Hall
                    {
                        Name = "Kisterem 2",
                        RowCount = 8,
                        ColumnCount = 10,
                    }
                ];

            List<Show> defaultShows = [];
            foreach (Movie movie in defaultMovies)
            {
                Int32 showCount = Random.Next(2, 10 + 1);
                for (int i = 0; i < showCount; i++)
                {
                    DateTime from = DateTime.Today.AddDays(-1);
                    DateTime to = DateTime.Today.AddDays(1);
                    Int32 showRange = (to - from).Days * 24;
                    DateTime showStart = from.AddHours(Random.Next(showRange));
                    defaultShows.Add(new Show
                    {
                        Start = showStart,
                        Movie = movie,
                        Hall = defaultHalls[Random.Next(defaultHalls.Count())]
                    });
                }
            }

            List<Seat> defaultSeats = [];
            foreach (Show show in defaultShows)
                for (int i = 0; i < show.Hall.RowCount; i++)
                    for (int j = 0; j < show.Hall.ColumnCount; j++)
                        defaultSeats.Add(new Seat
                        {
                            Show = show,
                            Hall = show.Hall,
                            Row = i + 1,
                            Column = j + 1,
                        });

            await context.AddRangeAsync(defaultMovies);
            await context.AddRangeAsync(defaultHalls);
            await context.AddRangeAsync(defaultShows);
            await context.AddRangeAsync(defaultSeats);

            await context.SaveChangesAsync();
        }
    }
}