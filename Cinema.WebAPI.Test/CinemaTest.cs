using Cinema.Data.Models;
using Cinema.Data.Models.DTOs;
using Cinema.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.WebAPI.Test
{
    public class CinemaTest : IDisposable
    {
        private readonly CinemaDbContext _context;
        private readonly HallsController _hallsController;
        private readonly MoviesController _moviesController;
        private readonly SeatsController _seatsController;
        private readonly ShowsController _showsController;

        public CinemaTest()
        {
            var options = new DbContextOptionsBuilder<CinemaDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _context = new CinemaDbContext(options);
            TestDbInitializer.InitializeAsync(_context);

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
