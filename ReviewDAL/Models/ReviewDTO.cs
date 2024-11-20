using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewDAL.Models
{
    [Table("Review")]
    public class ReviewDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? Text { get; set; }

        public int Rating { get; set; }
    }
}
