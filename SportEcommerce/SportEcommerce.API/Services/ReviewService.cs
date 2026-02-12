using SportEcommerce.API.Models.DTOs;
using SportEcommerce.API.Models.Entities;
using SportEcommerce.API.Repositories;

namespace SportEcommerce.API.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IProductRepository _productRepo;

        public ReviewService(
            IReviewRepository reviewRepo,
            IProductRepository productRepo)
        {
            _reviewRepo = reviewRepo;
            _productRepo = productRepo;
        }

        public async Task<IEnumerable<ReviewDTO>> GetProductReviewsAsync(int productId)
        {
            var reviews = await _reviewRepo.GetApprovedReviewsByProductIdAsync(productId);

            return reviews.Select(r => new ReviewDTO
            {
                Id = r.Id,
                ProductId = r.ProductId,
                UserName = r.User.FullName,
                Rating = r.Rating,
                Title = r.Title,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            });
        }

        public async Task<ReviewDTO> CreateReviewAsync(int userId, CreateReviewDTO reviewDto)
        {
            // Check if product exists
            var product = await _productRepo.GetByIdAsync(reviewDto.ProductId);
            if (product == null || !product.IsActive)
                throw new Exception("Product not found");

            // Check if user already reviewed this product
            var existingReview = await _reviewRepo.GetUserReviewForProductAsync(userId, reviewDto.ProductId);
            if (existingReview != null)
                throw new Exception("You have already reviewed this product");

            var review = new Review
            {
                ProductId = reviewDto.ProductId,
                UserId = userId,
                Rating = reviewDto.Rating,
                Title = reviewDto.Title,
                Comment = reviewDto.Comment,
                CreatedAt = DateTime.UtcNow,
                IsApproved = true // Auto-approve for now
            };

            var createdReview = await _reviewRepo.AddAsync(review);

            return new ReviewDTO
            {
                Id = createdReview.Id,
                ProductId = createdReview.ProductId,
                UserName = createdReview.User?.FullName ?? "Anonymous",
                Rating = createdReview.Rating,
                Title = createdReview.Title,
                Comment = createdReview.Comment,
                CreatedAt = createdReview.CreatedAt
            };
        }

        public async Task<bool> DeleteReviewAsync(int userId, int reviewId)
        {
            var review = await _reviewRepo.GetByIdAsync(reviewId);

            if (review == null || review.UserId != userId)
                return false;

            await _reviewRepo.DeleteAsync(review);
            return true;
        }

        public async Task<bool> UserHasReviewedProductAsync(int userId, int productId)
        {
            var review = await _reviewRepo.GetUserReviewForProductAsync(userId, productId);
            return review != null;
        }
    }
}