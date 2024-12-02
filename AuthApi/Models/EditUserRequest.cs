namespace AuthApi.Models
{
    public class EditUserRequest
    {
        public int Id { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
