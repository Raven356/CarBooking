namespace CarBookingUI.Models.Responses.OrderResponse
{
    public class OrderModel
    {
        public int Id { get; set; }

        public bool IsAcepted { get; set; }

        public OrderInfoModel RentInfo { get; set; }
    }
}
