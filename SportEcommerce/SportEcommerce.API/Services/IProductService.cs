using SportEcommerce.API.Models.DTOs;
using SportEcommerce.API.Models.Entities;

namespace SportEcommerce.API.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductDTO>> GetFeaturedProductsAsync();
        Task<IEnumerable<ProductDTO>> SearchProductsAsync(string searchTerm);
        Task<ProductDTO> CreateProductAsync(CreateProductDTO productDto);
        Task<ProductDTO> UpdateProductAsync(int id, CreateProductDTO productDto);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<ProductDTO>> GetProductsWithPaginationAsync(int page, int pageSize);
    }
}