using CinemaOnline.Data.DatabaseContext;
using CinemaOnline.Models.CinemaModels;
using Microsoft.EntityFrameworkCore;

namespace CinemaOnline.Data.Services.Cart
{
    /// <summary>
    /// Класс, описывающей карзину покупок билетов в кино
    /// </summary>
    public class ShoppingCart
    {
        private readonly CinemaDBContext _context;

        public string ShoppingCartId { get; init; }

        private IQueryable<ShoppingCartItem> _ItemsInCart { get => _context.ShoppingCartItems
            .Where(item => item.ShoppingCartId == ShoppingCartId);
        }

        public ShoppingCart(CinemaDBContext context, string shoppingCartId = null!)
        {
            _context = context;
            ShoppingCartId = shoppingCartId;
        }

        /// <summary>
        /// Получить карзину текущего пользователя
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            var session = services.GetService<IHttpContextAccessor>()?.HttpContext?.Session;
            if (session == null) throw new NullReferenceException($"current session : {nameof(session)};  is null");
            var context = services.GetService<CinemaDBContext>()!;
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            return new ShoppingCart(context,cartId);
            
        }
        private async Task<ShoppingCartItem> GetCartItemByMovie(Movie movie)
        {
            if (movie is null) throw new NullReferenceException($"{nameof(movie)} is null");
            var shoppingCartItem = await _context.ShoppingCartItems
                 .FirstOrDefaultAsync(item =>
                 item.Movie.Id == movie.Id &&
                 item.ShoppingCartId == ShoppingCartId);
            return shoppingCartItem ?? null!;
        }

        public async Task AddItemToCartAsync(Movie movie)
        {
            var shoppingCartItem = await GetCartItemByMovie(movie);
            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Movie = movie,
                    Amount = 1
                };
                await _context.ShoppingCartItems.AddAsync(shoppingCartItem);
            }
            else
                shoppingCartItem.Amount++;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemFromCartAsync(Movie movie)
        {
            var shoppingCartItem = await GetCartItemByMovie(movie);
            if (shoppingCartItem == null) 
                throw new NullReferenceException("shoppingcart is null");
           
            if (shoppingCartItem.Amount > 1)
            {
                shoppingCartItem.Amount--;
            }
            else
            {
                _context.ShoppingCartItems.Remove(shoppingCartItem);
            }
           
           await _context.SaveChangesAsync();
        }

        public async Task<double> GetShoppingCartTotalPrice()
        {
            if(_ItemsInCart is null )
                throw new NullReferenceException("shoppingcart is null");
            var movies = _ItemsInCart.Select(item => item.Movie);
            if(movies is null ||  movies.Count() <= 0 )
                throw new NullReferenceException("shoppingcart has zero  movies in it");
            // if(i)
            foreach (var movie in movies)
                if (movie.Price < 0)
                    throw new InvalidOperationException($"{movie.Name} \nprice is lesser then zero ");
            return await _ItemsInCart.Select(n => n.Movie.Price * n.Amount).SumAsync();
        }

        public async Task<List<ShoppingCartItem>> GetShoppingCartItemsAsync()
            =>  await _ItemsInCart.Include(cart => cart.Movie).ToListAsync();


        public async Task ClearShoppingCartAsync()
        {
            _context.ShoppingCartItems.RemoveRange(_ItemsInCart);
            await _context.SaveChangesAsync();
        }
    }
}
