using CarBookingUI.Helpers;
using CarBookingUI.Models;
using CarBookingUI.Models.Requests.OrderRequests;
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

                bool hasReview = false;

                var response = await HttpHelper.GetAsync($"http://10.0.2.2:8300/review/GetByOrderId?orderId={viewModel.Order.Id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var review = JsonConvert.DeserializeObject<Review>(content);

                    if (review != null) 
                    {
                        hasReview = true;
                    }
                }
                else
                {
                    throw new ArgumentException($"Error status code: {response.StatusCode}");
                }

                viewModel.CanWriteReview = (viewModel.Order.RentToUTC < DateTime.UtcNow || viewModel.Order.RentFinished != null) && !hasReview;
				viewModel.CanEndOrder = viewModel.Order.RentFinished == null;
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

    private async void OnEditOrderButtonClicked(object sender, EventArgs e)
	{
		try
		{
			await Navigation.PushAsync(new EditOrderPage(orderId));
		}
		catch (Exception ex)
		{
            await DisplayAlert("Error", $"Something went wrong: {ex.Message}!", "OK");
        }
	}

    private async void OnWriteOrderButtonClicked(object sender, EventArgs e)
	{
        try
        {
            await Navigation.PushAsync(new CreateReviewPage(viewModel.Order.Id, viewModel.Order.Image));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Something went wrong: {ex.Message}!", "OK");
        }
    }

    private async void OnEndOrderButtonClicked(object sender, EventArgs e)
	{
		try
		{
			var endOrderRequest = new EndOrderRequest
			{
				OrderId = orderId,
				FinishedTime = DateTime.UtcNow,
			};

			var response = await HttpHelper.PostAsJsonAsync("http://10.0.2.2:8300/order/EndOrder", endOrderRequest);

			if (response.IsSuccessStatusCode)
			{
				viewModel.CanEndOrder = false;
				viewModel.CanWriteReview = true;
				await DisplayAlert("Ended", "Order successfully ended!", "OK");
			}
			else
			{
                await DisplayAlert("Error", $"Something went wrong: {response.StatusCode}!", "OK");
            }
		}
		catch (Exception ex)
		{
            await DisplayAlert("Error", $"Something went wrong: {ex.Message}!", "OK");
        }
	}

    private async void OnMenuClicked(object sender, EventArgs e)
    {
        string action = await DisplayActionSheet("Choose an action", "Cancel", null, "Logout", "Order history", "Main page", "Review");

        switch (action)
        {
            case "Logout":
                await Logout();
                await Navigation.PopToRootAsync();
                break;
            case "Main page":
                await Navigation.PushAsync(new MainPage());
                break;
            case "Order history":
                await Navigation.PushAsync(new HistoryPage());
                break;
            case "Review":
                await Navigation.PushAsync(new ReviewsPage());
                break;
            default:
                // Cancel or null case
                break;
        }
    }

    private async Task Logout()
    {
        try
        {
            var response = await HttpHelper.PostAsync($"http://10.0.2.2:8300/api/v1/Auth/logout?userId={await SecureStorage.GetAsync("userId")}");
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException($"Error, status code: {response.StatusCode}");
            }
            else
            {
                SecureStorage.RemoveAll();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
}