namespace CarBookingUI.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? Text { get; set; }

        public int Rating { get; set; }

        public int CarId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
