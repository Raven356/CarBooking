using CarBookingUI.Models.Responses.OrderResponse;

namespace CarBookingUI.ViewModels
{
    public class HistoryOrderDetailsPageViewModel : BaseViewModel
    {
        private OrderCarResponse order;

        private bool canWriteReview;

        private bool canEndOrder;

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

        public bool CanWriteReview
        {
            get => canWriteReview;
            set
            {
                canWriteReview = value;
                OnPropertyChanged();
            }
        }

        public bool CanEndOrder
        {
            get => canEndOrder;
            set
            {
                canEndOrder = value;
                OnPropertyChanged();
            }
        }

        public HistoryOrderDetailsPageViewModel()
        {
            Order = new OrderCarResponse();
        }
    }
}
