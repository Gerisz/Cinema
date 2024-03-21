using Cinema.Data.Models.Tables;
using Cinema.Data.Models.Tables.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Cinema.Data.Models
{

    public static class DbInitializer
    {
        private static readonly Random _random = new();

        public static async Task InitializeAsync(CinemaDbContext context, String imageDirectory)
        {
            await context.Database.MigrateAsync();

            if (context.Shows.Any(s => s.Start.Date == DateTime.Today))
                return;

            var images = Directory.GetFiles(imageDirectory);

            List<Movie> defaultMovies =
                JsonSerializer.Deserialize<List<Movie>>(
                    File.ReadAllText("Models\\DefaultValues\\movies.json")
                ) ?? [];

            Int32 index = 0;

            defaultMovies
                .OrderBy(m => m.Title)
                .ToList()
                .ForEach(m => m.Image = File.Exists(images[index])
                    ? File.ReadAllBytes(images[index++]) : null);

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
                Int32 showCount = _random.Next(2, 10 + 1);
                for (int i = 0; i < showCount; i++)
                {
                    DateTime from = DateTime.Today.AddDays(-1);
                    DateTime to = DateTime.Today.AddDays(1);
                    Int32 showRange = (to - from).Days * 24;
                    DateTime showStart = from.AddHours(_random.Next(showRange));
                    defaultShows.Add(new Show
                    {
                        Start = showStart,
                        Movie = movie,
                        Hall = defaultHalls[_random.Next(defaultHalls.Count())]
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
                            Status = _random.Next(10) == 9 ? Status.Reserved : Status.Free
                        });

            await context.AddRangeAsync(defaultMovies);
            await context.AddRangeAsync(defaultHalls);
            await context.AddRangeAsync(defaultShows);
            await context.AddRangeAsync(defaultSeats);

            await context.SaveChangesAsync();
        }
    }
}