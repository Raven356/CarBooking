using ReviewBLL.Models;

namespace ReviewBLL.Interfaces
{
    public interface IReviewService
    {
        public Task<IEnumerable<Review>> GetAllAsync();

        public Task<IEnumerable<Review>> GetByUserIdAsync(int userId);

        public Task CreateReviewAsync(Review review);

        public Task<Review?> GetByIdAsync(int id);

        public Task UpdateReviewAsync(Review review);
    }
}
