namespace CarBookingUI.Models.Requests.ReviewRequests
{
    public class UpdateReviewRequest
    {
        public string? Text { get; set; }

        public int Rating { get; set; }

        public int Id { get; set; }
    }
}
