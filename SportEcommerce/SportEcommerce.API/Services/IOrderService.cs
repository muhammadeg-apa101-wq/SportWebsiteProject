using SportEcommerce.API.Models.DTOs;

namespace SportEcommerce.API.Services
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(int userId, CreateOrderDTO orderDto);
        Task<IEnumerable<OrderDTO>> GetUserOrdersAsync(int userId);
        Task<OrderDTO?> GetOrderByIdAsync(int userId, int orderId);
        Task<bool> CancelOrderAsync(int userId, int orderId);
    }
}
