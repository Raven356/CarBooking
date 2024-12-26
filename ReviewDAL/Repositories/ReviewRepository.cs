using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReviewDAL.Context;
using ReviewDAL.Interfaces;
using ReviewDAL.Models;

namespace ReviewDAL.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly IServiceProvider serviceProvider;

        public ReviewRepository(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task CreateReviewAsync(ReviewDTO reviewDTO)
        {
            using var context = CreateContext();

            reviewDTO.CreatedDate = DateTime.Now;
            await context.AddAsync(reviewDTO);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ReviewDTO>> GetAllAsync()
        {
            using var context = CreateContext();

            return await context.Reviews.ToListAsync();
        }

        public async Task<ReviewDTO?> GetByIdAsync(int id)
        {
            using var context = CreateContext();

            var review = await context.Reviews.SingleOrDefaultAsync(rev => rev.Id == id);

            return review;
        }

        public async Task<ReviewDTO> GetByOrderIdAsync(int orderId)
        {
            using var context = CreateContext();

            var review = await context.Reviews.FirstOrDefaultAsync(rev => rev.OrderId == orderId);

            return review;
        }

        public async Task<IEnumerable<ReviewDTO>> GetByUserIdAsync(int userId)
        {
            using var context = CreateContext();

            var reviews = await context.Reviews.Where(rev => rev.UserId == userId).ToListAsync();

            return reviews;
        }

        public async Task UpdateReviewAsync(ReviewDTO reviewDTO)
        {
            using var context = CreateContext();

            var existingReview = await context.Reviews.FindAsync(reviewDTO.Id);

            if (existingReview != null)
            {
                existingReview.Text = reviewDTO.Text;
                existingReview.Rating = reviewDTO.Rating;
                existingReview.CreatedDate = DateTime.Now;

                context.Update(existingReview);

                await context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Review with ID {reviewDTO.Id} not found.");
            }
        }

        private ReviewContext CreateContext()
        {
            var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ReviewContext>();
            context.Database.EnsureCreated();

            return context;
        }
    }
}
