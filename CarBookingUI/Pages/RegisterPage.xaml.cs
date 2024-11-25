using CarBookingUI.Models.Requests.AuthRequests;
using System.Net.Http.Json;

namespace CarBookingUI.Pages;

public partial class RegisterPage : ContentPage
{
    private readonly HttpClient _httpClient;

    public RegisterPage()
    {
        InitializeComponent();
        _httpClient = new HttpClient(InsecureHandler.InsecureHandler.GetInsecureHandler());
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        try
        {
            // Collect data from the UI
            var userModel = new UserRegisterRequest
            {
                Name = NameEntry.Text,
                Surname = SurnameEntry.Text,
                Phone = PhoneEntry.Text,
                Email = EmailEntry.Text,
                DateOfBirth = DateOnly.FromDateTime(DateOfBirthPicker.Date),
                Login = LoginEntry.Text,
                Password = PasswordEntry.Text
            };

            // Make the API call
            var response = await _httpClient.PostAsJsonAsync("http://10.0.2.2:8300/api/v1/Auth/register", userModel);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Success", "Registration successful!", "OK");
                await Navigation.PushAsync(new LoginPage());
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", $"Registration failed: {error}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
}