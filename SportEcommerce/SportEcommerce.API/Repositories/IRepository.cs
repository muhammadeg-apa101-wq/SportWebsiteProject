using System.Linq.Expressions;

namespace SportEcommerce.API.Repositories
{
    public interface IRepository<T> where T : class
    {
        // Get operations
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Add operations
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        // Update operations
        Task UpdateAsync(T entity);

        // Delete operations
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);

        // Other operations
        Task<bool> ExistsAsync(int id);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    }
}