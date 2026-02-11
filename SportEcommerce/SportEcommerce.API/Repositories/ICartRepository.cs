using SportEcommerce.API.Models.Entities;

namespace SportEcommerce.API.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);
        Task<Cart?> GetCartWithItemsAsync(int userId);
        Task<CartItem?> GetCartItemAsync(int cartId, int productId);
        Task ClearCartAsync(int cartId);
    }
}