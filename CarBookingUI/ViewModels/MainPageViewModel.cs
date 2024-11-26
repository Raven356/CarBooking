using CarBookingUI.Models;
using System.Collections.ObjectModel;

namespace CarBookingUI.ViewModels
{
    public class MainPageViewModel
    {
        public ObservableCollection<Car> Cars { get; set; }

        public bool IsAuthVisible { get; set; }

        public MainPageViewModel(IEnumerable<Car> cars)
        {
            Cars = new ObservableCollection<Car>(cars);

            IsAuthVisible = true;
            //IsAuthVisible = SecureStorage.GetAsync("auth_token").GetAwaiter().GetResult() == null;
        }
    }
}
