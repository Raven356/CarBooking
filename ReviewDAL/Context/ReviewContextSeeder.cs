namespace ReviewDAL.Context
{
    public class ReviewContextSeeder
    {
        public static void SeedData(ReviewContext context)
        {
            //context.Database.Migrate();

            // Check if data already exists
            if (!context.Reviews.Any())
            {
                context.Reviews.Add(new Models.ReviewDTO
                {
                    Rating = 4,
                    Text = "Text",
                    UserId = 1
                });

                context.SaveChanges();
            }
        }
    }
}
