using CarBookingUI.Helpers;
using CarBookingUI.Models.Requests.UserRequests;
using CarBookingUI.Models.Responses.UserResponses;
using CarBookingUI.ViewModels;
using Newtonsoft.Json;

namespace CarBookingUI.Pages;

public partial class EditUserPage : ContentPage
{
	private EditUserViewModel viewModel;

	public EditUserPage(int userId)
	{
		InitializeComponent();
		viewModel = new EditUserViewModel(userId);
		BindingContext = viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

		try
		{
            var response = await HttpHelper.GetAsync($"http://10.0.2.2:8300/api/v1/User/GetUserById?userId={viewModel.Id}");

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
                DateOfBirth.Date = user.DateOfBirth.ToDateTime(TimeOnly.MinValue);
            }
			else
			{
				throw new ArgumentException($"Something went wrong, status code: {response.StatusCode}");
			}
        }
		catch (Exception ex)
		{
            await DisplayAlert("Error", $"Something went wrong: {ex.Message}!", "OK");
        }
    }

	private async void OnConfirmButtonClicked(object sender, EventArgs e)
	{
		try
		{
			var request = new EditUserRequest
			{
				Id = viewModel.Id,
				DateOfBirth = DateOfBirth.Date.ToString(),
				Email = viewModel.Email,
				Name = viewModel.Name,
				Password = viewModel.Password,
				Phone = viewModel.Phone,
				Surname = viewModel.Surname
			};

			var response = await HttpHelper.PostAsJsonAsync("http://10.0.2.2:8300/api/v1/User/EditUser", request);

			if (response.IsSuccessStatusCode)
			{
				await DisplayAlert("Success", "Information edited successfully", "Ok");
                await Navigation.PushAsync(new MainPage());
            }
			else
			{
				throw new ArgumentException($"Something went wrong, status code: {response.StatusCode}");
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", $"Something went wrong: {ex.Message}!", "OK");
		}
	}
}