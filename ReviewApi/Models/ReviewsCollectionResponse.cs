using ReviewBLL.Models;

namespace ReviewApi.Models
{
    public class ReviewsCollectionResponse
    {
        public IEnumerable<Review> Reviews { get; set; }
    }
}
