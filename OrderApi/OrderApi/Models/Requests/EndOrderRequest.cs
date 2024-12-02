namespace OrderApi.Models.Requests
{
    public class EndOrderRequest
    {
        public int OrderId { get; set; }

        public DateTime FinishedTime { get; set; }
    }
}
