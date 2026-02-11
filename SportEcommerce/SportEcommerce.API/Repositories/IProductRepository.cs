using SportEcommerce.API.Models.Entities;

namespace SportEcommerce.API.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetFeaturedProductsAsync();
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
        Task<Product?> GetProductWithReviewsAsync(int productId);
        Task<IEnumerable<Product>> GetProductsWithPaginationAsync(int page, int pageSize);
    }
}