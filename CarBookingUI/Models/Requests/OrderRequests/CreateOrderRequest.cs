namespace CarBookingUI.Models.Requests.OrderRequests
{
    public class CreateOrderRequest : BaseOrderRequest
    {
        public int CarId { get; set; }

        public int UserId { get; set; }
    }
}
