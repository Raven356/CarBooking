using ReviewDAL.Models;

namespace ReviewDAL.Interfaces
{
    public interface IReviewRepository
    {
        public Task<IEnumerable<ReviewDTO>> GetAllAsync();

        public Task<IEnumerable<ReviewDTO>> GetByUserIdAsync(int userId);

        public Task CreateReviewAsync(ReviewDTO reviewDTO);

        public Task<ReviewDTO?> GetByIdAsync(int id);

        public Task UpdateReviewAsync(ReviewDTO reviewDTO);
    }
}
