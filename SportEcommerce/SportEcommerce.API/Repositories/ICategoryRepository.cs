using SportEcommerce.API.Models.Entities;

namespace SportEcommerce.API.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetCategoryWithProductsAsync(int categoryId);
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
    }
}