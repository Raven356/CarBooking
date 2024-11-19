using System.ComponentModel.DataAnnotations.Schema;

namespace AuthDAL.Models
{
    [Table("Token")]
    public class TokenDTO
    {
        public int Id { get; set; }

        public TypeEnum Type { get; set; }

        public required string Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public int UserId { get; set; }

        public UserDTO User { get; set; }
    }
}
