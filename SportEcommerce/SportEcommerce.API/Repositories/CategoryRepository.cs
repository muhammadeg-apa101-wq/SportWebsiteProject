using Microsoft.EntityFrameworkCore;
using SportEcommerce.API.Data;
using SportEcommerce.API.Models.Entities;

namespace SportEcommerce.API.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Category?> GetCategoryWithProductsAsync(int categoryId)
        {
            return await _dbSet
                .Include(c => c.Products.Where(p => p.IsActive))
                .FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _dbSet
                .Where(c => c.Products.Any(p => p.IsActive))
                .ToListAsync();
        }
    }
}