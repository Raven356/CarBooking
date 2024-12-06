namespace CarBookingUI.Models.Requests.ReviewRequests
{
    public class CreateReviewRequest : BaseReviewRequest
    {
        public int UserId { get; set; }

        public int OrderId { get; set; }
    }
}
