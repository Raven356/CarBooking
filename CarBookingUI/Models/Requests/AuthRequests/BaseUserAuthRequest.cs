namespace CarBookingUI.Models.Requests.AuthRequests
{
    internal abstract class BaseUserAuthRequest
    {
        public required string Login { get; set; }

        public required string Password { get; set; }
    }
}
