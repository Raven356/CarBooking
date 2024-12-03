namespace OrderApi.Models.Responses
{
    public class CarResponse
    {
        public int Id { get; set; }

        public required string CarPlate { get; set; }

        public string Type { get; set; }

        public string Model { get; set; }

        public double RentPrice { get; set; }

        public int? RentBy { get; set; }

        public int CarId { get; set; }
    }
}
