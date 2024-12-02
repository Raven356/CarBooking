using CarBookingUI.Models;
using System.Collections.ObjectModel;

namespace CarBookingUI.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private bool isAuthVisible;

        private ObservableCollection<Car> cars;

        public bool IsAuthVisible 
        { 
            get => isAuthVisible;
            set
            {
                isAuthVisible = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Car> Cars
        {
            get => cars;
            set
            {
                cars = value;
                OnPropertyChanged();
            }
        }

        public MainPageViewModel()
        {
            
        }
    }
}
