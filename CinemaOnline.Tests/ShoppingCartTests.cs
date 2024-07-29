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
        private readonly ShoppingCart cart;
       
        public ShoppingCartTests()
        {
            var _dbContextOptionsBuilder = new DbContextOptionsBuilder<CinemaDBContext>();
            _dbContextOptionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
 
            _context = new CinemaDBContext(_dbContextOptionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

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
            cart = ShoppingCart.GetShoppingCart(services);

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
        public async Task AddToNotEmptyShoppingCartOneValidMovie_Cart_ShouldHave_One_More_Movie()
        {
            //Arrange
            var movie = A.Fake<Movie>();
            movie.Description = Guid.NewGuid().ToString();
            movie.Name = Guid.NewGuid().ToString();
            movie.ImageURL = Guid.NewGuid().ToString();
            var carItemsBeforeAdd = _context.ShoppingCartItems.FirstOrDefault(x => x.ShoppingCartId == cart.ShoppingCartId)?.Amount ?? 0;
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
            var ItemsAmountShouldBecomeInCart = carItemsBeforeAdd + 1;
            var cartItems = await cart.GetShoppingCartItemsAsync();
            var result = cartItems.Count;
            Assert.Equal(ItemsAmountShouldBecomeInCart, result);
            



        }

        [Fact]
        public async Task RemoveFromEmptyShoppingCartOneMovie_Throws_NullException()
        {
            //Arrange
            var movie = A.Fake<Movie>();
            movie.Description = Guid.NewGuid().ToString();
            movie.Name = Guid.NewGuid().ToString();
            movie.ImageURL = Guid.NewGuid().ToString();

            //Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(async () => await cart.RemoveItemFromCartAsync(movie));


        }


        [Fact]
        public async Task RemoveFromNotEmptyShoppingCartOneMovie_ShouldDecrementAmount()
        {
            //Arrange
            var movie = A.Fake<Movie>();
            movie.Description = Guid.NewGuid().ToString();
            movie.Name = Guid.NewGuid().ToString();
            movie.ImageURL = Guid.NewGuid().ToString();
            await cart.AddItemToCartAsync(movie);
            await cart.AddItemToCartAsync(movie);
            var carItemsBeforeRemove = _context.ShoppingCartItems.FirstOrDefault(x => x.ShoppingCartId == cart.ShoppingCartId)?.Amount;

            //Act  
            await cart.RemoveItemFromCartAsync(movie);

            //Assert
            var ItemsAmountShouldLeftInCart = carItemsBeforeRemove - 1;
            var carItems = _context.ShoppingCartItems.FirstOrDefault(x => x.ShoppingCartId == cart.ShoppingCartId);
            var carItemsAfterRemove = carItems?.Amount;
            Assert.Equal(ItemsAmountShouldLeftInCart, carItemsAfterRemove);



        }


        [Fact]
        public async Task RemoveFromNotEmptyShoppingCartOneMovie_ShouldRemoveCart()
        {
            //Arrange
            var movie = A.Fake<Movie>();
            movie.Description = Guid.NewGuid().ToString();
            movie.Name = Guid.NewGuid().ToString();
            movie.ImageURL = Guid.NewGuid().ToString();
            await cart.AddItemToCartAsync(movie);
            var carItemsBeforeRemove = _context.ShoppingCartItems.FirstOrDefault(x => x.ShoppingCartId == cart.ShoppingCartId)?.Amount;

            //Act  
            await cart.RemoveItemFromCartAsync(movie);

            //Assert
            var cartItems = _context.ShoppingCartItems.FirstOrDefault(x => x.ShoppingCartId == cart.ShoppingCartId);
            cartItems.Should().BeNull();
        }


        [Fact]
        public async Task EmptyCartTotalPrice_ShouldThrowException()
        {
            //Arrange
            //var expectedTotalPrice = 0D;

            //Act & Assert            
            await Assert.ThrowsAsync<NullReferenceException>(async () =>await cart.GetShoppingCartTotalPrice());

        }

        [Theory]
        [InlineData(100,10)]
        [InlineData(20,132)]
        [InlineData(120,123)]
       // [InlineData(-5)]
        public async Task CartTotalPrice_Should_Return_TotalSum(double moviePrice,int movieCount)
        {
            //Arrange
           
            var expectedTotalPrice = Math.Abs(moviePrice) * movieCount;
            var movie = A.Fake<Movie>();
            movie.Description = Guid.NewGuid().ToString();
            movie.Name = Guid.NewGuid().ToString();
            movie.ImageURL = Guid.NewGuid().ToString();
            movie.Price = moviePrice;
            for (int i = 0; i < movieCount; i++)
            {
                await cart.AddItemToCartAsync(movie);
            }


            //Act
            var resultPrice = await cart.GetShoppingCartTotalPrice();

            //Arrange
            resultPrice.Should().Be(expectedTotalPrice);

        }


        [Theory]
        [InlineData(-1, 10)]
        [InlineData(-5, 132)]
        [InlineData(-532, 123)]
        public async Task CartTotalPrice_WithNegativePrice_Should_Throw_InvalidOperation(double moviePrice, int movieCount)
        {
            //Arrange

            var expectedTotalPrice = Math.Abs(moviePrice) * movieCount;
            var movie = A.Fake<Movie>();
            movie.Description = Guid.NewGuid().ToString();
            movie.Name = Guid.NewGuid().ToString();
            movie.ImageURL = Guid.NewGuid().ToString();
            movie.Price = moviePrice;
            for (int i = 0; i < movieCount; i++)
            {
                await cart.AddItemToCartAsync(movie);
            }

            //Act & Arrange
          await Assert.ThrowsAsync<InvalidOperationException>(async () => await cart.GetShoppingCartTotalPrice());
 

        }


        [Fact]
        public async Task ClearShoppingCart_Must_Act_JustOneTime()
        {
            //Arrange
            var movie = A.Fake<Movie>();
            movie.Description = Guid.NewGuid().ToString();
            movie.Name = Guid.NewGuid().ToString();
            movie.ImageURL = Guid.NewGuid().ToString();
            //for (int i = 0; i < movieCount; i++)
            //{
            //    movie.Price = (double)i;
            //    await cart.AddItemToCartAsync(movie);
            //}

        }
    }
}
