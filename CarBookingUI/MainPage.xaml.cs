using CarBookingUI.Helpers;
using CarBookingUI.Mappers;
using CarBookingUI.Models;
using CarBookingUI.Models.Responses.CarResponses;
using CarBookingUI.Models.Responses.UserResponses;
using CarBookingUI.Pages;
using CarBookingUI.Popups;
using CarBookingUI.ViewModels;
using CommunityToolkit.Maui.Views;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

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

            await GetUserIdAsync();

            await GetCarCollectionAsync();
        }

        private async Task GetUserIdAsync()
        {
            if (await SecureStorage.GetAsync("userId") == null)
            {
                try
                {
                    var userResponse = await HttpHelper.GetAsync($"http://10.0.2.2:8300/api/v1/User/GetUserIdByToken?token={await SecureStorage.GetAsync("auth_token")}");
                    if (userResponse.IsSuccessStatusCode)
                    {
                        var responseContent = await userResponse.Content.ReadAsStringAsync();
                        var userIdResponse = JsonConvert.DeserializeObject<UserIdResponse>(responseContent);

                        await SecureStorage.SetAsync("userId", userIdResponse.UserId.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Thread.Sleep(15000);
                    await GetUserIdAsync();
                }
            }
        }

        private async Task OnLoginClicked()
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private async void OnCarTapped(object sender, ItemTappedEventArgs e)
        {
            if (viewModel.IsAuthVisible)
            {
                await DisplayAlert("Unauthorized!", "Authorize to interact with app!", "Ok!");
                return;
            }

            if (e.Item is Car selectedCar)
            {
                var viewModel = new OrderPageViewModel()
                {
                    CarId = selectedCar.Id,
                    Description = $"Are you sure you want to proceed with ordering car {selectedCar.Name} for price {selectedCar.Price}",
                    Name = selectedCar.Name,
                    Image = selectedCar.Image
                };

                await Navigation.PushAsync(new OrderPage(viewModel));
            }

            // Знімаємо виділення елемента
            ((ListView)sender).SelectedItem = null;
        }

        private async void OnReloadClicked(object sender, EventArgs e)
        {
            await GetUserIdAsync();
            await GetCarCollectionAsync();
        }

        private async void OnFilterClicked(object sender, EventArgs e)
        {
            var models = await GetModelsCollectionAsync();
            var types = await GetTypesCollectionAsync();

            if (models != null && types != null)
            {
                var popup = new CarFilterPopup(models, types);
                var result = await this.ShowPopupAsync(popup);

                if (result is not null)
                {
                    var filters = (dynamic)result;

                    await GetCarCollectionAsync(filters.Type, filters.PriceFrom, filters.PriceTo, filters.Model);
                }
            }
            else
            {
                await DisplayAlert("Error", "Models ot types were null", "Ok");
            }
        }

        private async void OnMenuClicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Choose an action", "Cancel", null, viewModel.IsAuthVisible ? "Login" : "Logout", "Order history", "Review", "User cabinet");

            switch (action)
            {
                case "Logout":
                    await Logout();
                    break;
                case "Login":
                    await OnLoginClicked();
                    break;
                case "Order history":
                    if (viewModel.IsAuthVisible)
                    {
                        await DisplayAlert("Unauthorized!", "Authorize to interact with app!", "Ok!");
                        return;
                    }
                    await Navigation.PushAsync(new HistoryPage());
                    break;
                case "Review":
                    if (viewModel.IsAuthVisible)
                    {
                        await DisplayAlert("Unauthorized!", "Authorize to interact with app!", "Ok!");
                        return;
                    }
                    await Navigation.PushAsync(new ReviewsPage());
                    break;
                case "User cabinet":
                    if (viewModel.IsAuthVisible)
                    {
                        await DisplayAlert("Unauthorized!", "Authorize to interact with app!", "Ok!");
                        return;
                    }
                    await Navigation.PushAsync(new UserDetailsPage());
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
                SecureStorage.RemoveAll();
                viewModel.IsAuthVisible = true;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task<IEnumerable<string>> GetTypesCollectionAsync()
        {
            try
            {
                var response = await HttpHelper.GetAsync("http://10.0.2.2:8300/car/GetAllTypes");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<IEnumerable<string>>(content);
                }
                else
                {
                    throw new ArgumentException($"Error, status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }

            return null;
        }

        private async Task<IEnumerable<string>> GetModelsCollectionAsync()
        {
            try
            {
                var response = await HttpHelper.GetAsync("http://10.0.2.2:8300/car/GetAllModels");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<IEnumerable<string>>(content);
                }
                else
                {
                    throw new ArgumentException($"Error, status code: {response.StatusCode}");
                }
            }
            catch (Exception ex) 
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }

            return null;
        }

        private async Task GetCarCollectionAsync(string? type = null, string? fromPrice = null, string? toPrice = null, string? model = null)
        {
            try
            {
                var response = await HttpHelper.GetAsync($"http://10.0.2.2:8300/car/GetAll?type={type}&fromPrice={fromPrice}" +
                    $"&toPrice={toPrice}&model={model}&userId={await SecureStorage.GetAsync("userId")}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    viewModel.Cars = new ObservableCollection<Car>(
                        CarMapper.Map(
                            JsonConvert.DeserializeObject<IEnumerable<CarResponse>>(content)
                        )
                    );

                    viewModel.IsAuthVisible = SecureStorage.GetAsync("auth_token").GetAwaiter().GetResult() == null;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}
