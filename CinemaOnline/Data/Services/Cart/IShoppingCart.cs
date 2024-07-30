using CinemaOnline.Models.CinemaModels;

namespace CinemaOnline.Data.Services.Cart
{
    public interface IShoppingCart
    {
        string ShoppingCartId { get; init; }

        Task AddItemToCartAsync(Movie movie);
        Task ClearShoppingCartAsync();
        Task<List<ShoppingCartItem>> GetShoppingCartItemsAsync();
        Task<double> GetShoppingCartTotalPrice();
        Task RemoveItemFromCartAsync(Movie movie);
    }
}