using System.ComponentModel.DataAnnotations.Schema;

namespace CarBookingDAL.Models
{
    [Table("CarModel")]
    public class CarModelDTO
    {
        public int Id { get; set; }

        public string Model { get; set; }
    }
}
