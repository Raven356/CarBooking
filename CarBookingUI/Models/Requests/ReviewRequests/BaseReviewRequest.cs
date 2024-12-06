namespace CarBookingUI.Models.Requests.ReviewRequests
{
    public abstract class BaseReviewRequest
    {
        public string? Text { get; set; }

        public int Rating { get; set; }
    }
}
