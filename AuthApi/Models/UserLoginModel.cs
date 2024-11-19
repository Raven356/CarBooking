namespace AuthApi.Models
{
    public class UserLoginModel
    {
        public required string Login { get; set; }

        public required string Password { get; set; }

        public string? AccessToken { get; set; }

    }
}
