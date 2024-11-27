using CarBookingUI.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarBookingUI.ViewModels
{
    public class HistoryPageViewModel : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
