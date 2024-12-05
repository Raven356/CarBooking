using CarBookingUI.ViewModels;
using CommunityToolkit.Maui.Views;

namespace CarBookingUI.Popups;

public partial class CarFilterPopup : Popup
{
	public CarFilterPopup(IEnumerable<string> models, IEnumerable<string> types)
	{
		InitializeComponent();
		BindingContext = new CarFilterPopupViewModel(this, models, types);
	}
}