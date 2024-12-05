using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CarBookingUI.ViewModels
{
    class CarFilterPopupViewModel : BaseViewModel
    {
        public ObservableCollection<string> Models { get; set; }
        public ObservableCollection<string> Types { get; set; }
        public string SelectedModel { get; set; }
        public string PriceFrom { get; set; }
        public string PriceTo { get; set; }
        public string SelectedType { get; set; }

        public ICommand ApplyFilterCommand { get; }
        public ICommand CloseCommand { get; }

        private readonly Popup _popup;

        public CarFilterPopupViewModel(Popup popup, IEnumerable<string> models, IEnumerable<string> types)
        {
            _popup = popup;

            Models = new ObservableCollection<string>(models);
            Types = new ObservableCollection<string>(types);
            SelectedType = "";
            SelectedModel = "";
            PriceFrom = "";
            PriceTo = "";

            ApplyFilterCommand = new Command(ApplyFilter);
            CloseCommand = new Command(() => _popup.Close());
        }

        private void ApplyFilter()
        {
            var filters = new
            {
                Model = SelectedModel,
                PriceFrom,
                PriceTo,
                Type = SelectedType
            };

            // Apply filter logic (e.g., pass filters to a parent view model)
            _popup.Close(filters);
        }
    }
}
