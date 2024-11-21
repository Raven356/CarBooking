namespace OrderBLL.Models
{
    public class RentOrder
    {
        public int Id { get; set; }

        public bool IsAcepted { get; set; }

        public RentInfo RentInfo { get; set; }
    }
}
