using System.ComponentModel.DataAnnotations.Schema;

namespace OrderDAL.Models
{
    [Table("RentInfo")]
    public class RentInfoDTO
    {
        public int Id { get; set; }

        public DateTime RentFromUTC { get; set; }

        public DateTime RentToUTC { get; set; }

        public int CarId { get; set; }

        public int RentBy {  get; set; }
    }
}
