namespace OrderBLL.Models
{
    public class RentInfo
    {
        public int Id { get; set; }

        public DateTime RentFromUTC { get; set; }

        public DateTime RentToUTC { get; set; }

        public int CarId { get; set; }

        public int RentBy { get; set; }
    }
}
