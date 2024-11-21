using System.ComponentModel.DataAnnotations.Schema;

namespace OrderDAL.Models
{
    [Table("RentOrder")]
    public class RentOrderDTO
    {
        public int Id { get; set; }

        public bool IsAcepted { get; set; }

        public int RentInfoId { get; set; }

        public RentInfoDTO RentInfoDTO { get; set; }
    }
}
