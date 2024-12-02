namespace CarBookingUI.ViewModels
{
    public class UserDetailsViewModel : BaseViewModel
    {
        private string phone;
        private string name;
        private string surname;
        private string email;
        private DateOnly dateOfBirth;

        public int Id { get; set; }

        public string Phone { get => phone; set { phone = value; OnPropertyChanged(); } }

        public string Name { get => name; set { name = value; OnPropertyChanged(); } }

        public string Surname { get => surname; set { surname = value; OnPropertyChanged(); } }

        public string Email { get => email; set { email = value; OnPropertyChanged(); } }

        public DateOnly DateOfBirth { get => dateOfBirth; set { dateOfBirth = value; OnPropertyChanged(); } }
    }
}
