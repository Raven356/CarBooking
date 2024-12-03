using ReviewApi.Models;
using ReviewBLL.Models;

namespace ReviewApi.Mappers
{
    public static class ReviewMapper
    {
        public static Review Map(CreateReviewModel model)
        {
            return new Review
            {
                Rating = model.Rating,
                Text = model.Text,
                UserId = model.UserId,
                CarId = model.CarId
            };
        }

        public static Review Map(UpdateReviewModel model)
        {
            return new Review
            {
                Id = model.Id,
                Rating = model.Rating,
                Text = model.Text,
                UserId = model.UserId
            };
        }
    }
}
