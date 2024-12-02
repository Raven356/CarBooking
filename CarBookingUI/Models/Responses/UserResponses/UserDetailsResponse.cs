namespace CarBookingUI.Models.Responses.UserResponses
{
    public class UserDetailsResponse
    {
        public int Id { get; set; }

        public string Phone { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public DateOnly DateOfBirth { get; set; }
    }
}
