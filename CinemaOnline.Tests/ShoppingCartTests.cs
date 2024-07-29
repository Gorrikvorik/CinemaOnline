using CinemaOnline.Data.DatabaseContext;
using CinemaOnline.Data.Services.Cart;
using CinemaOnline.Models.CinemaModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CinemaOnline.Tests
{
    public class ShoppingCartTests
    {
        private readonly CinemaDBContext _context;
       
        public ShoppingCartTests()
        {
            var _dbContextOptionsBuilder = new DbContextOptionsBuilder<CinemaDBContext>();
            _dbContextOptionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            _context = new CinemaDBContext(_dbContextOptionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

        }

        [Fact]
        public void GetShopingCart_CurrntSessionIsNull_ThrowsException()
        {
            //Arrange
            var services = A.Fake<IServiceProvider>();
            var httpContext = A.Fake<HttpContext>();
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            A.CallTo(() => services.GetService(typeof(IHttpContextAccessor)))
                .Returns(httpContextAccessor);
            httpContextAccessor.HttpContext = httpContext;
            httpContextAccessor.HttpContext.Session = null!;


            //Act & Assert
            Assert.Throws<NullReferenceException>(() => ShoppingCart.GetShoppingCart(services));

        }
        [Fact]
        public void GetShoppingCart_ValidSession_ReturnsShoppingCart()
        {
            //Arrange
            var services = A.Fake<IServiceProvider>();
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var httpContext = A.Fake<HttpContext>();
            A.CallTo(() => services.GetService(typeof(IHttpContextAccessor)))
                .Returns(httpContextAccessor);
            httpContextAccessor.HttpContext = A.Fake<HttpContext>();
            var session = A.Fake<ISession>();
            A.CallTo(() => services.GetService(typeof(IHttpContextAccessor)))
              .Returns(httpContextAccessor);
            A.CallTo(() => services.GetService(typeof(CinemaDBContext)))
                .Returns(_context);
            httpContext.Session = session;
            httpContextAccessor.HttpContext = httpContext;

            //act
            var cart = ShoppingCart.GetShoppingCart(services);


            //Assert
            cart.Should().NotBeNull();
            

        }


        private ShoppingCart FakeShoppingCart()
        {
            var services = A.Fake<IServiceProvider>();
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var httpContext = A.Fake<HttpContext>();
            A.CallTo(() => services.GetService(typeof(IHttpContextAccessor)))
                .Returns(httpContextAccessor);
            httpContextAccessor.HttpContext = A.Fake<HttpContext>();
            var session = A.Fake<ISession>();
            A.CallTo(() => services.GetService(typeof(IHttpContextAccessor)))
              .Returns(httpContextAccessor);
            A.CallTo(() => services.GetService(typeof(CinemaDBContext)))
                .Returns(_context);
            httpContext.Session = session;
            httpContextAccessor.HttpContext = httpContext;

          return  ShoppingCart.GetShoppingCart(services);
        }

        [Fact]
        public async Task AddToEmptyShoppingCartOneValidMovie_Returns_Void()
        {
            //Arrange
            var cart = FakeShoppingCart();
            var movie = A.Fake<Movie>();
            movie.Description = Guid.NewGuid().ToString();
            movie.Name = Guid.NewGuid().ToString();
            movie.ImageURL = Guid.NewGuid().ToString();

            //act
            await cart.AddItemToCartAsync(movie);

            //Assert
            var itemsInCart = await cart.GetShoppingCartItemsAsync();
            itemsInCart.Should().NotBeNull();
            itemsInCart.Count.Should().Be(1);


        }

        [Fact]
        public async Task AddToNotEmptyShoppingCartOneValidMovie_Returns_Void()
        {
            //Arrange
            var cart = FakeShoppingCart();
            var movie = A.Fake<Movie>();
            movie.Description = Guid.NewGuid().ToString();
            movie.Name = Guid.NewGuid().ToString();
            movie.ImageURL = Guid.NewGuid().ToString();
            var carItemsBeforeAdded = _context.ShoppingCartItems.FirstOrDefault(x => x.ShoppingCartId == cart.ShoppingCartId)?.Amount ?? 0;
            _context.ShoppingCartItems.Add(new ShoppingCartItem
            {
                Amount = 1,
                Movie = movie,
                ShoppingCartId = cart.ShoppingCartId
            });
            await _context.SaveChangesAsync();

            //act
            await cart.AddItemToCartAsync(movie);

            //Assert
            var cartItems = await cart.GetShoppingCartItemsAsync();
            var result = cartItems.Count;
            Assert.Equal(1, result - carItemsBeforeAdded);
            



        }
    }
}
