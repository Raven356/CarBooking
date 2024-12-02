namespace OrderApi.Models.Requests
{
    public class CreateOrderRequest
    {
        public int CarId { get; set; }

        public int UserId { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
    }
}
