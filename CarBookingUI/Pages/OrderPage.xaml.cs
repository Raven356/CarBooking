using CarBookingUI.Helpers;
using CarBookingUI.Models.Responses.UserResponses;
using CarBookingUI.ViewModels;
using Newtonsoft.Json;

namespace CarBookingUI.Pages;

public partial class OrderPage : ContentPage
{
    private int carId;

    public OrderPage(OrderPageViewModel viewModel)
	{
		InitializeComponent();

        carId = viewModel.CarId;
        BindingContext = viewModel;
    }

    private async void OnConfirmButtonClicked(object sender, EventArgs e)
    {
        try
        {
            var userResponse = await HttpHelper.GetAsync($"http://10.0.2.2:8300/api/v1/User/GetUserIdByToken?token={await SecureStorage.GetAsync("auth_token")}");
            if (userResponse.IsSuccessStatusCode)
            {
                var responseContent = await userResponse.Content.ReadAsStringAsync();
                var userIdResponse = JsonConvert.DeserializeObject<UserIdResponse>(responseContent);
                var orderResponse = await HttpHelper.PostAsync($"http://10.0.2.2:8300/order/CreateOrder?carId={carId}&userId={userIdResponse.UserId}");

                if (orderResponse.IsSuccessStatusCode)
                {
                    await DisplayAlert("Order Created", "Your car has been booked!", "OK");
                }
                else 
                {
                    throw new ArgumentException("Order request had errors!");
                }
            }
        }
        catch (Exception ex) 
        {
            await DisplayAlert("Order error", $"Something went wrong, error: {ex.Message}", "OK");
        }
    }
}