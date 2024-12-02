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

        private MainPageViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
            viewModel = new MainPageViewModel();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await GetCarCollectionAsync();
        }

        private async Task OnLoginClicked()
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private async void OnCarTapped(object sender, ItemTappedEventArgs e)
        {
            //if (viewModel.IsAuthVisible)
            //{
            //    await DisplayAlert("Unauthorized!", "Authorize to interact with app!", "Ok!");
            //}

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

        private async void OnReloadClicked(object sender, EventArgs e)
        {
            await GetCarCollectionAsync();
        }

        private async void OnMenuClicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Choose an action", "Cancel", null, "Login", "Order history", "Review", "User cabinet");

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
                case "User cabinet":
                    await Navigation.PushAsync(new UserDetailsPage());
                    break;
                default:
                    // Cancel or null case
                    break;
            }
        }

        private async Task GetCarCollectionAsync()
        {
            try
            {
                var response = HttpHelper.Get("http://10.0.2.2:8300/car/GetAll");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    viewModel.Cars = new System.Collections.ObjectModel.ObservableCollection<Car>(
                        CarMapper.Map(
                            JsonConvert.DeserializeObject<IEnumerable<CarResponse>>(content)
                        )
                    );

                    viewModel.IsAuthVisible = true;
                    //viewModel.IsAuthVisible = SecureStorage.GetAsync("auth_token").GetAwaiter().GetResult() == null;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK").GetAwaiter();
            }
        }
    }
}
