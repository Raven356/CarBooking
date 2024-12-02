namespace CarBookingUI.Models.Requests.OrderRequests
{
    public class EndOrderRequest
    {
        public int OrderId { get; set; }

        public DateTime FinishedTime { get; set; }
    }
}
