namespace CarBookingUI.ViewModels
{
    public class EditUserViewModel : BaseViewModel
    {
        private int id;
        private string password;
        private string phone;
        private string name;
        private string surname;
        private string email;
        private DateOnly dateOfBirth;

        public EditUserViewModel(int id)
        {
            Id = id;
        }

        public int Id { get => id; set { id = value; OnPropertyChanged(); } }

        public string Password { get => password; set { password = value; OnPropertyChanged(); } }

        public string Phone { get => phone; set { phone = value; OnPropertyChanged(); } }

        public string Name { get => name; set { name = value; OnPropertyChanged(); } }

        public string Surname { get => surname; set { surname = value; OnPropertyChanged(); } }

        public string Email { get => email; set { email = value; OnPropertyChanged(); } }

        public DateOnly DateOfBirth { get => dateOfBirth; set { dateOfBirth = value; OnPropertyChanged(); } }
    }
}
