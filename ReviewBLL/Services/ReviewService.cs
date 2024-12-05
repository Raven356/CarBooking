using ReviewBLL.Interfaces;
using ReviewBLL.Mappers;
using ReviewBLL.Models;
using ReviewDAL.Interfaces;

namespace ReviewBLL.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository repository;

        public ReviewService(IReviewRepository repository)
        {
            this.repository = repository;
        }

        public async Task CreateReviewAsync(Review review)
        {
            await repository.CreateReviewAsync(ReviewMapper.Map(review));
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return ReviewMapper.Map(await repository.GetAllAsync());
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            var review = await repository.GetByIdAsync(id);
            return review == null ? null : ReviewMapper.Map(review);
        }

        public async Task<Review> GetByOrderIdAsync(int orderId)
        {
            var review = await repository.GetByOrderIdAsync(orderId);
            return review != null ? ReviewMapper.Map(review) : null;
        }

        public async Task<IEnumerable<Review>> GetByUserIdAsync(int userId)
        {
            return ReviewMapper.Map(await repository.GetByUserIdAsync(userId));
        }

        public async Task UpdateReviewAsync(Review review)
        {
            await repository.UpdateReviewAsync(ReviewMapper.Map(review));
        }
    }
}
