namespace OrderApi.Models.Responses
{
    public class OrderCarResponse
    {
        public int Id { get; set; }

        public bool IsAcepted { get; set; }

        public DateTime RentFromUTC { get; set; }

        public DateTime RentToUTC { get; set; }

        public DateTime? RentFinished { get; set; }

        //public byte[] CarImage { get; set; }

        public string CarPlate { get; set; }
    }
}
