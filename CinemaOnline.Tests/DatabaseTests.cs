using CinemaOnline.Data.DatabaseContext;
using CinemaOnline.Models.CinemaModels;
using CinemaOnline.Models.OwnedModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CinemaOnline.Tests
{
    public class DatabaseTests
    {
        private  CinemaDBContext _context;
        private  DbContextOptionsBuilder<CinemaDBContext> _dbContextOptionsBuilder;
        public DatabaseTests()
        {

            _dbContextOptionsBuilder = new DbContextOptionsBuilder<CinemaDBContext>();
            _dbContextOptionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            _context = new CinemaDBContext(_dbContextOptionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

        }


        [Fact]
        public void CreateDatabase_Returns_Void()
        {
            //arrange
            _dbContextOptionsBuilder = new DbContextOptionsBuilder<CinemaDBContext>();
            _dbContextOptionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            _context = new CinemaDBContext(_dbContextOptionsBuilder.Options);
            _context.Database.EnsureDeleted();


            //act
            _context.Database.EnsureCreated();


            //assert
            Assert.NotNull(_context);
            Assert.NotNull(_context.Database);
        }

        [Fact]
        public void InserIntoActorsTable_OneActor_Returns_Void()
        {
            //arrange
            var fakeActor = A.Fake<Actor>();
            fakeActor.ProfilePictureURL = "http://example.com/profile.jpg";
            fakeActor.FullName = "Имя Фамилия";
            fakeActor.Bio = "Описание биографии актера";
            fakeActor.Rating = new Rating { Score = 5 };
            fakeActor.ActorsMovies = new List<ActorMovies>
                {
                    new ActorMovies { ActorId = 1, MovieId = 100,Role = "Убийца" },
                    new ActorMovies { ActorId = 1, MovieId = 101,Role = "Доктор"}
                };
            _context.Actors.Add(fakeActor);

            //act
            _context.SaveChanges();


            //assert

            _context.Actors.Should().HaveCount(1);
            Assert.Equal("http://example.com/profile.jpg", fakeActor.ProfilePictureURL);
            Assert.Equal("Имя Фамилия", fakeActor.FullName);
            Assert.Equal("Описание биографии актера", fakeActor.Bio);
            Assert.Equal(5, fakeActor.Rating.Score);
            Assert.Equal(2, fakeActor.ActorsMovies?.Count);
        }
    }
}