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

        public readonly string _shoppingCartId;

        public List<ShoppingCartItem>? ShoppingCartItems { get; init; }
        public ShoppingCart(CinemaDBContext context, string shoppingCartId = null!)
        {
            _context = context;
            _shoppingCartId = shoppingCartId;
        }

        /// <summary>
        /// Получить карзину текущего пользователя
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            var session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;
            if (session == null) throw new NullReferenceException($"current session : {nameof(session)};  is null");
            var context = services.GetRequiredService<CinemaDBContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            return new ShoppingCart(context,cartId);
            
        }
        private async Task<ShoppingCartItem> GetCartItem(Movie movie)
        {
            if (movie is null) throw new NullReferenceException($"{nameof(movie)} is null");
            var shoppingCartItem = await _context.ShoppingCartItems
                 .FirstOrDefaultAsync(item =>
                 item.Movie.Id == movie.Id &&
                 item.ShoppingCartId == _shoppingCartId);
            return shoppingCartItem ?? null!;
        }

        public async Task AddItemToCartAsync(Movie movie)
        {
            var shoppingCartItem = await GetCartItem(movie);
            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = _shoppingCartId,
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
            var shoppingCartItem = await GetCartItem(movie);
            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                }
                else
                {
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }
           await _context.SaveChangesAsync();
        }

        public async Task<double> GetShoppingCartTotalPrice() =>
            await _context.ShoppingCartItems
            .Where(item => item.ShoppingCartId == _shoppingCartId)
            .Select(n => n.Movie.Price * n.Amount)
            .SumAsync();

        public async Task<List<ShoppingCartItem>> GetShoppingCartItemsAsync()
            => ShoppingCartItems ?? await _context.ShoppingCartItems.Where(item => item.ShoppingCartId == _shoppingCartId).Include(cart => cart.Movie).ToListAsync();


        public async Task ClearShoppingCartAsync()
        {
            var items = await _context.ShoppingCartItems.Where(n => n.ShoppingCartId == _shoppingCartId).ToListAsync();
            _context.ShoppingCartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
