namespace AuthBLL.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string? Phone { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? Email { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public bool IsActive { get; set; }

        public RolesEnum Role { get; set; }
    }
}
