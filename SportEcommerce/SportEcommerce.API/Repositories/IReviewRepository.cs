using SportEcommerce.API.Models.Entities;

namespace SportEcommerce.API.Repositories
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId);
        Task<IEnumerable<Review>> GetApprovedReviewsByProductIdAsync(int productId);
        Task<Review?> GetUserReviewForProductAsync(int userId, int productId);
        Task<double> GetAverageRatingForProductAsync(int productId);
    }
}
