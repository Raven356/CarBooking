namespace AuthApi.Models
{
    public abstract class BaseUserAuthModel
    {
        public required string Login { get; set; }

        public required string Password { get; set; }
    }
}
