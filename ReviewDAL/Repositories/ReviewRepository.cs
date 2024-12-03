using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReviewDAL.Context;
using ReviewDAL.Interfaces;
using ReviewDAL.Models;

namespace ReviewDAL.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ReviewContext context;

        public ReviewRepository(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ReviewContext>();
            context.Database.EnsureCreated();
        }

        public async Task CreateReviewAsync(ReviewDTO reviewDTO)
        {
            reviewDTO.CreatedDate = DateTime.Now;
            await context.AddAsync(reviewDTO);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ReviewDTO>> GetAllAsync()
        {
            return await context.Reviews.ToListAsync();
        }

        public async Task<ReviewDTO?> GetByIdAsync(int id)
        {
            var review = await context.Reviews.SingleOrDefaultAsync(rev => rev.Id == id);

            return review;
        }

        public async Task<IEnumerable<ReviewDTO>> GetByUserIdAsync(int userId)
        {
            var reviews = await context.Reviews.Where(rev => rev.UserId == userId).ToListAsync();

            return reviews;
        }

        public async Task UpdateReviewAsync(ReviewDTO reviewDTO)
        {
            var existingReview = await context.Reviews.FindAsync(reviewDTO.Id);

            if (existingReview != null)
            {
                // Оновлення полів
                context.Entry(existingReview).CurrentValues.SetValues(reviewDTO);

                await context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Review with ID {reviewDTO.Id} not found.");
            }
        }
    }
}
