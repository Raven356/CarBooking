namespace AuthApi.Models
{
    public class UserRegisterModel : BaseUserAuthModel
    {
        public string? Phone { get; set; }

        public required string Name { get; set; }

        public required string Surname { get; set; }

        public required string Email { get; set; }

        public DateOnly DateOfBirth { get; set; }
    }
}
