using SportEcommerce.API.Data;
using SportEcommerce.API.Models.DTOs;
using SportEcommerce.API.Models.Entities;
using SportEcommerce.API.Repositories;

namespace SportEcommerce.API.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IProductRepository _productRepo;
        private readonly ApplicationDbContext _context;

        public CartService(
            ICartRepository cartRepo,
            IProductRepository productRepo,
            ApplicationDbContext context)
        {
            _cartRepo = cartRepo;
            _productRepo = productRepo;
            _context = context;
        }

        public async Task<CartDTO?> GetUserCartAsync(int userId)
        {
            var cart = await _cartRepo.GetCartWithItemsAsync(userId);

            if (cart == null)
                return null;

            return MapToCartDTO(cart);
        }

        public async Task<CartDTO> AddToCartAsync(int userId, AddToCartDTO addToCartDto)
        {
            // Check if product exists and is available
            var product = await _productRepo.GetByIdAsync(addToCartDto.ProductId);

            if (product == null || !product.IsActive)
                throw new Exception("Product not found or unavailable");

            if (product.StockQuantity < addToCartDto.Quantity)
                throw new Exception("Insufficient stock");

            // Get or create cart
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                cart = await _cartRepo.AddAsync(cart);
            }

            // Check if item already exists in cart
            var existingItem = await _cartRepo.GetCartItemAsync(cart.Id, addToCartDto.ProductId);

            if (existingItem != null)
            {
                // Update quantity
                existingItem.Quantity += addToCartDto.Quantity;

                if (product.StockQuantity < existingItem.Quantity)
                    throw new Exception("Insufficient stock");

                _context.CartItems.Update(existingItem);
            }
            else
            {
                // Add new item
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = addToCartDto.ProductId,
                    Quantity = addToCartDto.Quantity,
                    PriceAtAdd = product.DiscountPrice ?? product.Price,
                    AddedAt = DateTime.UtcNow
                };
                await _context.CartItems.AddAsync(cartItem);
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Return updated cart
            var updatedCart = await _cartRepo.GetCartWithItemsAsync(userId);
            return MapToCartDTO(updatedCart);
        }

        public async Task<CartDTO> UpdateCartItemAsync(int userId, int cartItemId, int quantity)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);

            if (cart == null)
                throw new Exception("Cart not found");

            var cartItem = await _context.CartItems.FindAsync(cartItemId);

            if (cartItem == null || cartItem.CartId != cart.Id)
                throw new Exception("Cart item not found");

            if (quantity <= 0)
                throw new Exception("Quantity must be greater than 0");

            // Check stock
            var product = await _productRepo.GetByIdAsync(cartItem.ProductId);
            if (product.StockQuantity < quantity)
                throw new Exception("Insufficient stock");

            cartItem.Quantity = quantity;
            cart.UpdatedAt = DateTime.UtcNow;

            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();

            var updatedCart = await _cartRepo.GetCartWithItemsAsync(userId);
            return MapToCartDTO(updatedCart);
        }

        public async Task<bool> RemoveFromCartAsync(int userId, int cartItemId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);

            if (cart == null)
                return false;

            var cartItem = await _context.CartItems.FindAsync(cartItemId);

            if (cartItem == null || cartItem.CartId != cart.Id)
                return false;

            _context.CartItems.Remove(cartItem);
            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ClearCartAsync(int userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);

            if (cart == null)
                return false;

            await _cartRepo.ClearCartAsync(cart.Id);
            return true;
        }

        // Helper method
        private CartDTO MapToCartDTO(Cart cart)
        {
            var items = cart.CartItems.Select(ci => new CartItemDTO
            {
                Id = ci.Id,
                ProductId = ci.ProductId,
                ProductName = ci.Product.Name,
                ProductImage = ci.Product.ImageUrl,
                Price = ci.PriceAtAdd,
                Quantity = ci.Quantity,
                Subtotal = ci.PriceAtAdd * ci.Quantity
            }).ToList();

            return new CartDTO
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = items,
                TotalAmount = items.Sum(i => i.Subtotal)
            };
        }
    }
}