using CarBookingUI.Helpers;
using CarBookingUI.Mappers;
using CarBookingUI.Models;
using CarBookingUI.Models.Responses.OrderResponse;
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
            var response = await HttpHelper.GetAsync($"http://10.0.2.2:8300/order/GetByUserId?userId={await SecureStorage.GetAsync("userId")}");

            if (response.IsSuccessStatusCode)
            {
                var orderContent = await response.Content.ReadAsStringAsync();
                var orderResponse = JsonConvert.DeserializeObject<IEnumerable<OrderModel>>(orderContent);

                viewModel.Orders = new System.Collections.ObjectModel.ObservableCollection<Models.Order>(OrderMapper.Map(orderResponse));
            }
        }
        catch (Exception ex) 
        {
            await DisplayAlert("Error", $"Something went wrong: {ex.Message}!", "OK");
        }
    }

    private async void OnMenuClicked(object sender, EventArgs e)
    {
        string action = await DisplayActionSheet("Choose an action", "Cancel", null, "Logout", "Main page", "Review");

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

    private async void OnOrderTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is Order selectedOrder)
        {
            await Navigation.PushAsync(new HistoryOrderDetailsPage(selectedOrder.Id));
        }

        // ������ �������� ��������
        ((ListView)sender).SelectedItem = null;
    }
}