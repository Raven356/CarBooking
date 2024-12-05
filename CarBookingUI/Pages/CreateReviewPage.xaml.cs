using CarBookingUI.Helpers;
using CarBookingUI.Models.Requests.ReviewRequests;
using CarBookingUI.Models.Responses.UserResponses;
using CarBookingUI.ViewModels;
using Newtonsoft.Json;

namespace CarBookingUI.Pages;

public partial class CreateReviewPage : ContentPage
{
	private readonly CreateReviewViewModel viewModel;

	public CreateReviewPage(int orderId)
	{
		InitializeComponent();
		viewModel = new CreateReviewViewModel(orderId);

		BindingContext = viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        viewModel.UserId = int.Parse(await SecureStorage.GetAsync("userId"));
    }

    private async void OnConfirmButtonClicked(object sender, EventArgs e)
	{
		try
		{
			var request = new CreateReviewRequest
			{
				UserId = viewModel.UserId,
				OrderId = viewModel.OrderId,
				Rating = viewModel.Rating,
				Text = Text.Text
			};

			var response = await HttpHelper.PostAsJsonAsync($"http://10.0.2.2:8300/review/Create", request);

			if (response.IsSuccessStatusCode)
			{
                await DisplayAlert("Success", $"Review created successfully!", "OK");
				await Navigation.PopAsync();
            }
			else
			{
                throw new ArgumentException($"Error, status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Something went wrong: {ex.Message}!", "OK");
        }
    }

    private void OnStarClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button)
        {
            if (button.CommandParameter is string parameter && int.TryParse(parameter, out int ratingValue))
            {
                viewModel.Rating = ratingValue;
            }
        }
    }
}