using Cinema.Data.Models;
using Cinema.Data.Models.DTOs;
using Cinema.Data.Models.Tables.Enums;
using Cinema.Data.Models.Tables;
using Cinema.WebAPI.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Cinema.WebAPI.Test
{
    public class CinemaTest : IDisposable
    {
        private readonly CinemaDbContext _context;
        private readonly HallsController _hallsController;
        private readonly MoviesController _moviesController;
        private readonly SeatsController _seatsController;
        private readonly ShowsController _showsController;

        private static readonly Random _random = new();

        public CinemaTest()
        {
            var options = new DbContextOptionsBuilder<CinemaDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _context = new CinemaDbContext(options);
            Task.Run(InitializeAsync).Wait();

            /* 
             * Ezzel az utasítással elengedjük az adatbázis objektumainak követését (tracking).
             * Ez a listák átnevezésének teszteléséhez szükséges, mivel egyébként a
             * PutList megpróbálna új objektumot létrehozni az adatbázisban.
             */
            _context.ChangeTracker.Clear();

            _hallsController = new HallsController(_context);
            _moviesController = new MoviesController(_context);
            _seatsController = new SeatsController(_context);
            _showsController = new ShowsController(_context);
        }

        [Fact(Skip = "Method used to initialize the database used by tests.")]
        public async Task InitializeAsync()
        {
            if (_context.Shows.Any(s => s.Start.Date == DateTime.Today))
                return;

            List<Movie> defaultMovies =
                JsonSerializer.Deserialize<List<Movie>>(
                    File.ReadAllText(
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        "Models\\DefaultValues\\movies.json"))
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

            await _context.AddRangeAsync(defaultMovies);
            await _context.AddRangeAsync(defaultHalls);
            await _context.AddRangeAsync(defaultShows);
            await _context.AddRangeAsync(defaultSeats);

            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task PostMovieTest()
        {
            // Act
            var newMovie = new MovieDTO
            {
                Title = "title",
                Director = "director",
                Synopsis = "synopsis",
                Length = 60,
                Entry = DateTime.UtcNow
            };
            var count = _context.Movies.Count();

            // Act
            var result = await _moviesController.PostMovieAsync(newMovie);

            // Assert
            var objectResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result);
            var content = Assert.IsAssignableFrom<MovieDTO>(objectResult.Value);
            Assert.Equal(count + 1, _context.Movies.Count());
        }

        [Fact]
        public async Task PostMovieWithEmptyTitleTest()
        {
            // Act
            var newMovie = new MovieDTO
            {
                Title = "",
                Director = "director",
                Synopsis = "synopsis",
                Length = 60
            };
            var count = _context.Movies.Count();

            // Act
            var result = await _moviesController.PostMovieAsync(newMovie);

            // Assert
            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.Equal(count, _context.Movies.Count());
        }

        [Fact]
        public async Task PostShowAsync()
        {
            // Act
            var newShow = new ShowDTO
            {
                Start = DateTime.UtcNow + new TimeSpan(30, 0, 0, 0),
                Movie = new MovieDTO { Id = _context.Movies.First().Id },
                Hall = new HallDTO { Id = _context.Halls.First().Id }
            };
            var count = _context.Shows.Count();

            // Act
            var result = await _showsController.PostShowAsync(newShow);

            // Assert
            var objectResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result);
            var content = Assert.IsAssignableFrom<ShowDTO>(objectResult.Value);
            Assert.Equal(count + 1, _context.Shows.Count());
        }


        [Fact]
        public async Task PostShowTwiceAsync()
        {
            // Act
            var newShow = new ShowDTO
            {
                Start = DateTime.UtcNow + new TimeSpan(30, 0, 0, 0),
                Movie = new MovieDTO { Id = _context.Movies.First().Id },
                Hall = new HallDTO { Id = _context.Halls.First().Id }
            };
            var count = _context.Shows.Count();

            // Act 1
            var result = await _showsController.PostShowAsync(newShow);

            // Assert 1
            var objectResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result);
            var content = Assert.IsAssignableFrom<ShowDTO>(objectResult.Value);
            Assert.Equal(count + 1, _context.Shows.Count());

            // Act 2
            result = await _showsController.PostShowAsync(newShow);

            // Assert 2
            Assert.IsAssignableFrom<CreatedAtActionResult>(result);
            Assert.Equal(count + 2, _context.Shows.Count());
        }

        [Fact]
        public async Task SellSeatAsync()
        {
            // Act
            var show = _context.Shows.First();

            // Act
            var result = await _seatsController.SellSeatAsync(show.Id);

            // Assert
            Assert.IsAssignableFrom<OkResult>(result);
        }


        [Fact]
        public async Task SellSeatTwiceAsync()
        {
            // Act
            var show = _context.Shows.First();

            // Act 1
            var result = await _seatsController.SellSeatAsync(show.Id);

            // Assert 1
            Assert.IsAssignableFrom<OkResult>(result);


            // Act 2
            result = await _seatsController.SellSeatAsync(show.Id);

            // Assert 2
            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }
    }
}
