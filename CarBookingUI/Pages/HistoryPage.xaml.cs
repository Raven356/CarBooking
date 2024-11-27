using CarBookingUI.Helpers;
using CarBookingUI.Mappers;
using CarBookingUI.Models.Responses.OrderResponse;
using CarBookingUI.Models.Responses.UserResponses;
using CarBookingUI.ViewModels;
using Newtonsoft.Json;

namespace CarBookingUI.Pages;

public partial class HistoryPage : ContentPage
{
    private HistoryPageViewModel viewModel;

    public HistoryPage()
	{
		InitializeComponent();
        viewModel = new HistoryPageViewModel();
        BindingContext = viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var userResponse = await HttpHelper.GetAsync($"http://10.0.2.2:8300/api/v1/User/GetUserIdByToken?token={await SecureStorage.GetAsync("auth_token")}");
            if (userResponse.IsSuccessStatusCode)
            {
                var responseContent = await userResponse.Content.ReadAsStringAsync();
                var userIdResponse = JsonConvert.DeserializeObject<UserIdResponse>(responseContent);

                var response = await HttpHelper.GetAsync($"http://10.0.2.2:8300/order/GetByUserId?userId={userIdResponse.UserId}");

                if (response.IsSuccessStatusCode)
                {
                    var orderContent = await response.Content.ReadAsStringAsync();
                    var orderResponse = JsonConvert.DeserializeObject<IEnumerable<OrderModel>>(orderContent);

                    viewModel.Orders = new System.Collections.ObjectModel.ObservableCollection<Models.Order>(OrderMapper.Map(orderResponse));
                }
            }
        }
        catch (Exception ex) 
        {
            await DisplayAlert("Error", $"Something went wrong: {ex.Message}!", "OK");
        }
    }

    private async void OnMenuClicked(object sender, EventArgs e)
    {
        string action = await DisplayActionSheet("Choose an action", "Cancel", null, "Login", "Order history", "Review");

        switch (action)
        {
            case "Logout":
                await DisplayAlert("Action", "Logout", "OK"); ;
                break;
            case "Main page":
                await Navigation.PushAsync(new MainPage());
                break;
            case "Review":
                await DisplayAlert("Action", "You selected Option 3", "OK");
                break;
            default:
                // Cancel or null case
                break;
        }
    }

    private async void OnOrderTapped(object sender, EventArgs e)
    {

    }
}