using SportEcommerce.API.Models.DTOs;

namespace SportEcommerce.API.Services
{
    public interface ICartService
    {
        Task<CartDTO?> GetUserCartAsync(int userId);
        Task<CartDTO> AddToCartAsync(int userId, AddToCartDTO addToCartDto);
        Task<CartDTO> UpdateCartItemAsync(int userId, int cartItemId, int quantity);
        Task<bool> RemoveFromCartAsync(int userId, int cartItemId);
        Task<bool> ClearCartAsync(int userId);
    }
}