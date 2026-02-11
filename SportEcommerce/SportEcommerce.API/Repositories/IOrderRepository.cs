using SportEcommerce.API.Models.Entities;

namespace SportEcommerce.API.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order?> GetOrderWithDetailsAsync(int orderId);
        Task<Order?> GetOrderByOrderNumberAsync(string orderNumber);
    }
}