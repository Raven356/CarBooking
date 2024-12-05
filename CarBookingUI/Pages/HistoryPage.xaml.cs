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
                await Logout();
                await Navigation.PopToRootAsync();
                break;
            case "Main page":
                await Navigation.PushAsync(new MainPage());
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

    private async void OnOrderTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is Order selectedOrder)
        {
            await Navigation.PushAsync(new HistoryOrderDetailsPage(selectedOrder.Id));
        }

        // Знімаємо виділення елемента
        ((ListView)sender).SelectedItem = null;
    }
}