using CarBookingUI.Models.Responses.OrderResponse;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarBookingUI.ViewModels
{
    public class HistoryOrderDetailsPageViewModel : INotifyPropertyChanged
    {
        private OrderCarResponse order;

        public OrderCarResponse Order
        {
            get => order;
            set
            {
                if (order != value)
                {
                    order = value;
                    OnPropertyChanged();
                }
            }
        }

        public HistoryOrderDetailsPageViewModel()
        {
            Order = new OrderCarResponse();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
