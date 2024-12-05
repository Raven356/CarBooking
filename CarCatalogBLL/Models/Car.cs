namespace CarBookingBLL.Models
{
    public class Car
    {
        public int Id { get; set; }

        public required string CarPlate { get; set; }

        public required CarType CarType { get; set; }

        public required CarModel Model { get; set; }

        public double RentPrice { get; set; }

        public int? RentBy { get; set; }

        public byte[] Image { get; set; }
    }
}
