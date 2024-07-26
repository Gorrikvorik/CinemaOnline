using CinemaOnline.Data.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CinemaOnline.Tests
{
    public class DatabaseTests
    {
        private readonly ApplicationDbContext _context;
        private  DbContextOptionsBuilder<ApplicationDbContext> _dbContextOptionsBuilder;



        [Fact]
        public void DataBaseConfiguredAndCreated()
        {
            //arrange
            _dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            _dbContextOptionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            var context = new ApplicationDbContext(_dbContextOptionsBuilder.Options);


            //act
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();


            //assert
            Assert.NotNull(_context);
            Assert.NotNull(_context.Database);
        }
    }
}