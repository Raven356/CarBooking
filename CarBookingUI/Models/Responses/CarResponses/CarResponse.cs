namespace CarBookingUI.Models.Responses.CarResponses
{
    internal class CarResponse
    {
        public int Id { get; set; }

        public required string CarPlate { get; set; }

        public required CarTypeResponse CarType { get; set; }

        public required CarModelResponse Model { get; set; }

        public double RentPrice { get; set; }

        public int? RentBy { get; set; }
    }
}
