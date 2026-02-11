using Microsoft.EntityFrameworkCore;
using SportEcommerce.API.Data;
using SportEcommerce.API.Models.Entities;

namespace SportEcommerce.API.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetFeaturedProductsAsync()
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.IsFeatured && p.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.IsActive &&
                    (p.Name.Contains(searchTerm) ||
                     p.Description.Contains(searchTerm) ||
                     p.Brand.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<Product?> GetProductWithReviewsAsync(int productId)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<IEnumerable<Product>> GetProductsWithPaginationAsync(int page, int pageSize)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}