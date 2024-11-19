using System.ComponentModel.DataAnnotations.Schema;

namespace AuthDAL.Models
{
    [Table("User")]
    public class UserDTO
    {
        public int Id { get; set; }

        public required string Login { get; set; }

        [Column("Password")]
        public required string PasswordHash { get; set; }

        public string? Phone { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? Email { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public bool IsActive { get; set; }

        public RolesEnum Role { get; set; }
    }
}
