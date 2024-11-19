namespace AuthBLL.Models
{
    public class TokenModel
    {
        public int Id { get; set; }

        public TypeEnum Type { get; set; }

        public required string Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public User User { get; set; }
    }
}
