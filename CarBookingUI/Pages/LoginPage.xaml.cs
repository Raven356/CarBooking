using CarBookingUI.Models.Requests.AuthRequests;
using CarBookingUI.Models.Responses.AuthResponses;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace CarBookingUI.Pages;

public partial class LoginPage : ContentPage
{
    private readonly HttpClient _httpClient;

    public LoginPage()
	{
		InitializeComponent();
        _httpClient = new HttpClient(InsecureHandler.InsecureHandler.GetInsecureHandler());
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        try
        {
            var loginModel = new UserLoginRequest
            {
                Login = LoginEntry.Text,
                Password = PasswordEntry.Text
            };

            var response = await _httpClient.PostAsJsonAsync("http://10.0.2.2:8300/api/v1/Auth/login", loginModel);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync(); 
                var tokenObj = JsonConvert.DeserializeObject<LoginSuccessfullResponse>(responseContent); 
                var token = tokenObj.Token;

                await SecureStorage.SetAsync("auth_token", token);

                await DisplayAlert("Login", "Logged in successfully!", "OK");

                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", $"Login failed: {error}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        // Navigate to the Register page
        await Navigation.PushAsync(new RegisterPage());
    }
}