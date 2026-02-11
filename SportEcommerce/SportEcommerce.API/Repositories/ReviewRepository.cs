using Microsoft.EntityFrameworkCore;
using SportEcommerce.API.Data;
using SportEcommerce.API.Models.Entities;

namespace SportEcommerce.API.Repositories
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId)
        {
            return await _dbSet
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetApprovedReviewsByProductIdAsync(int productId)
        {
            return await _dbSet
                .Include(r => r.User)
                .Where(r => r.ProductId == productId && r.IsApproved)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Review?> GetUserReviewForProductAsync(int userId, int productId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == productId);
        }

        public async Task<double> GetAverageRatingForProductAsync(int productId)
        {
            var reviews = await _dbSet
                .Where(r => r.ProductId == productId && r.IsApproved)
                .ToListAsync();

            if (!reviews.Any())
                return 0;

            return reviews.Average(r => r.Rating);
        }
    }
}