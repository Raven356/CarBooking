using CarBookingUI.Helpers;
using CarBookingUI.Mappers;
using CarBookingUI.Models;
using CarBookingUI.Models.Responses.CarResponses;
using CarBookingUI.Pages;
using CarBookingUI.ViewModels;
using Newtonsoft.Json;

namespace CarBookingUI
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            try
            {
                var response = HttpHelper.Get("http://10.0.2.2:8300/car/GetAll");

                if (response.IsSuccessStatusCode)
                {
                    BindingContext = new MainPageViewModel(CarMapper.Map(
                        JsonConvert.DeserializeObject<IEnumerable<CarResponse>>(
                            response.Content
                                .ReadAsStringAsync()
                                .GetAwaiter()
                                .GetResult()))
                        );
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    DisplayAlert("Car", $"Unauthorized!", "OK").GetAwaiter();
                }
            }
            catch (Exception ex) 
            {
                DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK").GetAwaiter();
            }
        }

        private async Task OnLoginClicked()
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private async void OnCarTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Car selectedCar)
            {
                var viewModel = new OrderPageViewModel()
                {
                    CarId = selectedCar.Id,
                    Description = $"Are you sure you want to proceed with ordering car {selectedCar.Name} for price {selectedCar.Price}",
                    Name = selectedCar.Name
                };

                await Navigation.PushAsync(new OrderPage(viewModel));
            }

            // Знімаємо виділення елемента
            ((ListView)sender).SelectedItem = null;
        }

        private async void OnMenuClicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Choose an action", "Cancel", null, "Login", "Order history", "Review");

            switch (action)
            {
                case "Login":
                    await OnLoginClicked();
                    break;
                case "Order history":
                    await Navigation.PushAsync(new HistoryPage());
                    break;
                case "Review":
                    await DisplayAlert("Action", "You selected Option 3", "OK");
                    break;
                default:
                    // Cancel or null case
                    break;
            }
        }
    }

}
