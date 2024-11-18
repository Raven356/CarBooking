using System.ComponentModel.DataAnnotations.Schema;

namespace CarBookingDAL.Models
{
    [Table("CarType")]
    public class CarTypeDTO
    {
        public int Id { get; set; }

        public string Type { get; set; }
    }
}
