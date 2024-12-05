using CarBookingUI.Helpers;
using CarBookingUI.Models.Responses.UserResponses;
using CarBookingUI.ViewModels;
using Newtonsoft.Json;

namespace CarBookingUI.Pages;

public partial class UserDetailsPage : ContentPage
{
	private UserDetailsViewModel viewModel;

	public UserDetailsPage()
	{
		InitializeComponent();
		viewModel = new UserDetailsViewModel();

		BindingContext = viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var response = await HttpHelper.GetAsync($"http://10.0.2.2:8300/api/v1/User/GetUserByToken?token={await SecureStorage.GetAsync("auth_token")}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserDetailsResponse>(content);

                viewModel.Surname = user.Surname;
                viewModel.DateOfBirth = user.DateOfBirth;
                viewModel.Email = user.Email;
                viewModel.Id = user.Id;
                viewModel.Name = user.Name;
                viewModel.Phone = user.Phone;
            }
            else
            {
                throw new ArgumentException($"Something went wrong status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Something went wrong: {ex.Message}!", "OK");
        }
    }

    private async void OnEditUserButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditUserPage(viewModel.Id));
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