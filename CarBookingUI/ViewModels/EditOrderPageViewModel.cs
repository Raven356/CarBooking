namespace CarBookingUI.ViewModels
{
    public class EditOrderPageViewModel : BaseViewModel
    {
        private int orderId;
        private string name;
        private string description;
        private DateTime dateTo;
        private TimeOnly timeTo;
        private DateTime dateFrom;
        private TimeOnly timeFrom;

        public int OrderId { get => orderId; set {  orderId = value; OnPropertyChanged(); } }

        public string Name { get => name; set { name = value; OnPropertyChanged(); } }

        public string Description { get => description; set { description = value; OnPropertyChanged(); } }

        public DateTime DateTo { get => dateTo; set { dateTo = value; OnPropertyChanged(); } }

        public TimeOnly TimeTo { get => timeTo; set { timeTo = value; OnPropertyChanged(); } }

        public DateTime DateFrom { get => dateFrom; set { dateFrom = value; OnPropertyChanged(); } }

        public TimeOnly TimeFrom { get => timeFrom; set { timeFrom = value; OnPropertyChanged(); } }
    }
}
