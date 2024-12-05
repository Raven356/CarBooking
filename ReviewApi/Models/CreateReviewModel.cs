namespace ReviewApi.Models
{
    public class CreateReviewModel : ReviewBaseModel
    {
        public int UserId { get; set; }

        public int OrderId { get; set; }
    }
}
