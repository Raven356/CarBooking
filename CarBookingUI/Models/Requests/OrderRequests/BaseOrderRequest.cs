namespace CarBookingUI.Models.Requests.OrderRequests
{
    public abstract class BaseOrderRequest
    {
        public DateTime DateTo { get; set; }

        public DateTime DateFrom { get; set; }
    }
}
