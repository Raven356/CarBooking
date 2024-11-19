namespace AuthApi.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public required string Login { get; set; }

        public required string Password { get; set; }

        public string? Phone { get; set; }

        public required string Name { get; set; }

        public required string Surname { get; set; }

        public required string Email { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public bool IsActive { get; set; }

        public required RolesEnum Role { get; set; }
    }
}
