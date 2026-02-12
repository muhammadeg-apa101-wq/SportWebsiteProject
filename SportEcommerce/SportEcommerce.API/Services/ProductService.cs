using AutoMapper;
using SportEcommerce.API.Models.DTOs;
using SportEcommerce.API.Models.Entities;
using SportEcommerce.API.Repositories;

namespace SportEcommerce.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IReviewRepository _reviewRepo;
        private readonly IMapper _mapper;

        public ProductService(
            IProductRepository productRepo,
            IReviewRepository reviewRepo,
            IMapper mapper)
        {
            _productRepo = productRepo;
            _reviewRepo = reviewRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _productRepo.GetAllAsync();
            var productDTOs = new List<ProductDTO>();

            foreach (var product in products.Where(p => p.IsActive))
            {
                var dto = await MapToProductDTO(product);
                productDTOs.Add(dto);
            }

            return productDTOs;
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            var product = await _productRepo.GetProductWithReviewsAsync(id);

            if (product == null || !product.IsActive)
                return null;

            return await MapToProductDTO(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _productRepo.GetProductsByCategoryAsync(categoryId);
            var productDTOs = new List<ProductDTO>();

            foreach (var product in products)
            {
                var dto = await MapToProductDTO(product);
                productDTOs.Add(dto);
            }

            return productDTOs;
        }

        public async Task<IEnumerable<ProductDTO>> GetFeaturedProductsAsync()
        {
            var products = await _productRepo.GetFeaturedProductsAsync();
            var productDTOs = new List<ProductDTO>();

            foreach (var product in products)
            {
                var dto = await MapToProductDTO(product);
                productDTOs.Add(dto);
            }

            return productDTOs;
        }

        public async Task<IEnumerable<ProductDTO>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<ProductDTO>();

            var products = await _productRepo.SearchProductsAsync(searchTerm);
            var productDTOs = new List<ProductDTO>();

            foreach (var product in products)
            {
                var dto = await MapToProductDTO(product);
                productDTOs.Add(dto);
            }

            return productDTOs;
        }

        public async Task<ProductDTO> CreateProductAsync(CreateProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            product.CreatedAt = DateTime.UtcNow;
            product.IsActive = true;

            var createdProduct = await _productRepo.AddAsync(product);
            return await MapToProductDTO(createdProduct);
        }

        public async Task<ProductDTO> UpdateProductAsync(int id, CreateProductDTO productDto)
        {
            var product = await _productRepo.GetByIdAsync(id);

            if (product == null)
                throw new Exception("Product not found");

            // Update fields
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.DiscountPrice = productDto.DiscountPrice;
            product.StockQuantity = productDto.StockQuantity;
            product.Brand = productDto.Brand;
            product.Size = productDto.Size;
            product.Color = productDto.Color;
            product.ImageUrl = productDto.ImageUrl;
            product.ImageGallery = productDto.ImageGallery;
            product.CategoryId = productDto.CategoryId;
            product.IsFeatured = productDto.IsFeatured;
            product.UpdatedAt = DateTime.UtcNow;

            await _productRepo.UpdateAsync(product);
            return await MapToProductDTO(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);

            if (product == null)
                return false;

            // Soft delete
            product.IsActive = false;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepo.UpdateAsync(product);

            return true;
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsWithPaginationAsync(int page, int pageSize)
        {
            var products = await _productRepo.GetProductsWithPaginationAsync(page, pageSize);
            var productDTOs = new List<ProductDTO>();

            foreach (var product in products)
            {
                var dto = await MapToProductDTO(product);
                productDTOs.Add(dto);
            }

            return productDTOs;
        }

        // Helper method
        private async Task<ProductDTO> MapToProductDTO(Product product)
        {
            var avgRating = await _reviewRepo.GetAverageRatingForProductAsync(product.Id);
            var reviews = await _reviewRepo.GetApprovedReviewsByProductIdAsync(product.Id);

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                StockQuantity = product.StockQuantity,
                Brand = product.Brand,
                Size = product.Size,
                Color = product.Color,
                ImageUrl = product.ImageUrl,
                ImageGallery = product.ImageGallery,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? "",
                IsFeatured = product.IsFeatured,
                AverageRating = avgRating,
                ReviewCount = reviews.Count()
            };
        }
    }
}