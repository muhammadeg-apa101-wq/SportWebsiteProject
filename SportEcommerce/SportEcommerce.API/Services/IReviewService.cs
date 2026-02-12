using SportEcommerce.API.Models.DTOs;

namespace SportEcommerce.API.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDTO>> GetProductReviewsAsync(int productId);
        Task<ReviewDTO> CreateReviewAsync(int userId, CreateReviewDTO reviewDto);
        Task<bool> DeleteReviewAsync(int userId, int reviewId);
        Task<bool> UserHasReviewedProductAsync(int userId, int productId);
    }
}
