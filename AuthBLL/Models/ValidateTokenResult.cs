namespace AuthBLL.Models
{
    public class ValidateTokenResult
    {
        public bool Success { get; set; }

        public TokenModel? Token { get; set; }
    }
}
