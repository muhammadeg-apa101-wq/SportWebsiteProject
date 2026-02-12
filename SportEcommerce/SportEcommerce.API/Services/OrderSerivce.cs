using SportEcommerce.API.Data;
using SportEcommerce.API.Models.DTOs;
using SportEcommerce.API.Models.Entities;
using SportEcommerce.API.Repositories;

namespace SportEcommerce.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly ICartRepository _cartRepo;
        private readonly IProductRepository _productRepo;

        public OrderService(
            IOrderRepository orderRepo,
            ICartRepository cartRepo,
            IProductRepository productRepo)
        {
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
            _productRepo = productRepo;
        }

        public async Task<OrderDTO> CreateOrderAsync(int userId, CreateOrderDTO orderDto)
        {
            // Get user's cart
            var cart = await _cartRepo.GetCartWithItemsAsync(userId);

            if (cart == null || !cart.CartItems.Any())
                throw new Exception("Cart is empty");

            // Verify stock for all items
            foreach (var item in cart.CartItems)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product.StockQuantity < item.Quantity)
                    throw new Exception($"Insufficient stock for {product.Name}");
            }

            // Create order
            var order = new Order
            {
                UserId = userId,
                OrderNumber = GenerateOrderNumber(),
                TotalAmount = cart.CartItems.Sum(ci => ci.PriceAtAdd * ci.Quantity),
                Status = "Pending",
                ShippingAddress = orderDto.ShippingAddress,
                PhoneNumber = orderDto.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            var createdOrder = await _orderRepo.AddAsync(order);

            // Create order items and update stock
            foreach (var cartItem in cart.CartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = createdOrder.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    PriceAtPurchase = cartItem.PriceAtAdd
                };

                await _context.OrderItems.AddAsync(orderItem);

                // Update product stock
                var product = await _productRepo.GetByIdAsync(cartItem.ProductId);
                product.StockQuantity -= cartItem.Quantity;
                await _productRepo.UpdateAsync(product);
            }

            await _context.SaveChangesAsync();

            // Clear cart
            await _cartRepo.ClearCartAsync(cart.Id);

            // Return order DTO
            var orderWithDetails = await _orderRepo.GetOrderWithDetailsAsync(createdOrder.Id);
            return MapToOrderDTO(orderWithDetails);
        }

        public async Task<IEnumerable<OrderDTO>> GetUserOrdersAsync(int userId)
        {
            var orders = await _orderRepo.GetOrdersByUserIdAsync(userId);
            return orders.Select(MapToOrderDTO);
        }

        public async Task<OrderDTO?> GetOrderByIdAsync(int userId, int orderId)
        {
            var order = await _orderRepo.GetOrderWithDetailsAsync(orderId);

            if (order == null || order.UserId != userId)
                return null;

            return MapToOrderDTO(order);
        }

        public async Task<bool> CancelOrderAsync(int userId, int orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);

            if (order == null || order.UserId != userId)
                return false;

            if (order.Status != "Pending")
                throw new Exception("Cannot cancel order that is already processing");

            order.Status = "Cancelled";
            order.UpdatedAt = DateTime.UtcNow;
            await _orderRepo.UpdateAsync(order);

            return true;
        }

        // Helper methods
        private string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        private OrderDTO MapToOrderDTO(Order order)
        {
            return new OrderDTO
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                PhoneNumber = order.PhoneNumber,
                CreatedAt = order.CreatedAt,
                Items = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    ProductImage = oi.Product.ImageUrl,
                    Quantity = oi.Quantity,
                    Price = oi.PriceAtPurchase,
                    Subtotal = oi.PriceAtPurchase * oi.Quantity
                }).ToList()
            };
        }

        private readonly ApplicationDbContext _context;

        public OrderService(
            IOrderRepository orderRepo,
            ICartRepository cartRepo,
            IProductRepository productRepo,
            ApplicationDbContext context)
        {
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
            _productRepo = productRepo;
            _context = context;
        }
    }
}