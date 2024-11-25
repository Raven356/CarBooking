using System.ComponentModel.DataAnnotations.Schema;

namespace CarBookingDAL.Models
{
    [Table("Car")]
    public class CarDTO
    {
        public int Id { get; set; }

        public required string CarPlate { get; set; }

        public int TypeId { get; set; }

        public CarTypeDTO CarType { get; set; }

        public int ModelId { get; set; }

        public CarModelDTO Model { get; set; }

        public double RentPrice { get; set; }

        public int? RentBy { get; set; }
    }
}
