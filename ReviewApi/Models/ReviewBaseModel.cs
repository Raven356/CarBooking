namespace ReviewApi.Models
{
    public abstract class ReviewBaseModel
    {
        public string? Text { get; set; }

        public int Rating { get; set; }
    }
}
