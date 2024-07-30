using CinemaOnline.Data.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CinemaOnline.Tests
{
    public abstract class BaseTest
    {
        protected readonly CinemaDBContext _context;

        public BaseTest()
        {
            var _dbContextOptionsBuilder = new DbContextOptionsBuilder<CinemaDBContext>();
            _dbContextOptionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            _context = new CinemaDBContext(_dbContextOptionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
           


        }
    }
}
