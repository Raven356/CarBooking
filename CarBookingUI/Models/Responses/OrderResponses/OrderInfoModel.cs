namespace CarBookingUI.Models.Responses.OrderResponse
{
    public class OrderInfoModel
    {
        public int Id { get; set; }

        public DateTime RentFromUTC { get; set; }

        public DateTime RentToUTC { get; set; }

        public int CarId { get; set; }

        public int RentBy { get; set; }
    }
}
