namespace CarBookingUI.Models.Requests.ReviewRequests
{
    public class CreateReviewRequest
    {
        public int UserId { get; set; }

        public string? Text { get; set; }

        public int Rating { get; set; }

        public int OrderId { get; set; }
    }
}
