using Cinema.Data.Models;
using Cinema.Data.Models.Tables.Enums;
using Cinema.Data.Models.Tables;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Cinema.WebAPI.Test
{
    public static class TestDbInitializer
    {
        private static readonly Random _random = new();

        public static async void InitializeAsync(CinemaDbContext context)
        {
            if (context.Shows.Any(s => s.Start.Date == DateTime.Today))
                return;

            List<Movie> defaultMovies =
                JsonSerializer.Deserialize<List<Movie>>(
                    File.ReadAllText(
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        "Models\\DefaultValues\\movies.json"))
                ) ?? [];

            Int32 index = 0;

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

        public static async Task SeedUsersAsync(UserManager<Employee> userManager)
        {
            var user = await userManager.FindByNameAsync("admin");
            // if user with said role doesn't exist
            if (user == null)
            {
                // then create a default user for that role
                user = new Employee()
                {
                    Name = "admin",
                    UserName = "admin"
                };
                await userManager.CreateAsync(user, "admin");
            }
        }
    }
}
