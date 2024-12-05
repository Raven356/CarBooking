﻿using ReviewBLL.Models;
using ReviewDAL.Models;

namespace ReviewBLL.Mappers
{
    internal static class ReviewMapper
    {
        public static ReviewDTO Map(Review review)
        {
            return new ReviewDTO
            {
                Id = review.Id,
                Rating = review.Rating,
                Text = review.Text,
                UserId = review.UserId,
                OrderId = review.OrderId,
                CreatedDate = review.CreatedDate,
            };
        }

        public static Review Map(ReviewDTO review)
        {
            return new Review
            {
                Id = review.Id,
                Rating = review.Rating,
                Text = review.Text,
                UserId = review.UserId,
                CreatedDate = review.CreatedDate,
                OrderId = review.OrderId,
            };
        }

        public static IEnumerable<Review> Map(IEnumerable<ReviewDTO> reviews)
        {
            return reviews.Select(Map);
        }
    }
}
