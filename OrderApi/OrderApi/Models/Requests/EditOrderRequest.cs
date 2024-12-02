namespace OrderApi.Models.Requests
{
    public class EditOrderRequest
    {
        public int Id { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
    }
}
