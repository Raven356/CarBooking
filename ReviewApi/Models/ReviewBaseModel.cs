namespace ReviewApi.Models
{
    public abstract class ReviewBaseModel
    {
        public int UserId { get; set; }

        public string? Text { get; set; }

        public int Rating { get; set; }
    }
}
