using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewDAL.Models
{
    [Table("Message")]
    public class MessageDTO
    {
        public int Id { get; set; }

        public required string Message { get; set; }

        public int UserId { get; set; }

        public int ChatId { get; set; }

        public ChatDTO Chat { get; set; }
    }
}
