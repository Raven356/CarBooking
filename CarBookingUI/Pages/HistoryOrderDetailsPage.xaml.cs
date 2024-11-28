using CarBookingUI.Helpers;
using CarBookingUI.Models.Responses.OrderResponse;
using CarBookingUI.ViewModels;
using Newtonsoft.Json;

namespace CarBookingUI.Pages;

public partial class HistoryOrderDetailsPage : ContentPage
{
	private readonly int orderId;
	private HistoryOrderDetailsPageViewModel viewModel;

	public HistoryOrderDetailsPage(int orderId)
	{
		InitializeComponent();
		this.orderId = orderId;
		viewModel = new HistoryOrderDetailsPageViewModel();
		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		try
		{
			var orderResponse = await HttpHelper.GetAsync($"http://10.0.2.2:8300/order/GetOrderById?orderId={orderId}");

			if (orderResponse.IsSuccessStatusCode)
			{
                var responseContent = await orderResponse.Content.ReadAsStringAsync();
                viewModel.Order = JsonConvert.DeserializeObject<OrderCarResponse>(responseContent);
            }
			else
			{
                await DisplayAlert("Error", $"Something went wrong: {orderResponse.StatusCode}!", "OK");
            }
        }
		catch (Exception ex)
		{
            await DisplayAlert("Error", $"Something went wrong: {ex.Message}!", "OK");
        }
	}
}