using System.Collections.ObjectModel;

namespace CarBookingUI.ViewModels
{
    public class HistoryPageViewModel : BaseViewModel
    {
        private ObservableCollection<Models.Order> _orders;

        public ObservableCollection<Models.Order> Orders
        {
            get => _orders;
            set
            {
                if (_orders != value)
                {
                    _orders = value;
                    OnPropertyChanged();
                }
            }
        }

        public HistoryPageViewModel()
        {
            Orders = new ObservableCollection<Models.Order>();
        }
    }
}
