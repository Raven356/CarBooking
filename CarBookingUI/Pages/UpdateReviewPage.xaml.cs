using CarBookingUI.Helpers;
using CarBookingUI.Models;
using CarBookingUI.Models.Requests.ReviewRequests;
using CarBookingUI.ViewModels;
using Newtonsoft.Json;

namespace CarBookingUI.Pages;

public partial class UpdateReviewPage : ContentPage
{
	private readonly EditReviewViewModel viewModel;

	public UpdateReviewPage(int orderId)
	{
		InitializeComponent();
		viewModel = new EditReviewViewModel(orderId);
		BindingContext = viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

		try
		{
			var response = await HttpHelper.GetAsync($"http://10.0.2.2:8300/review/GetById?reviewId={viewModel.ReviewId}");

			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				var review = JsonConvert.DeserializeObject<Review>(content);

				viewModel.Rating = review.Rating;
				viewModel.Text = review.Text;
				viewModel.UserId = review.UserId;
				viewModel.CarId = review.CarId;
			}
			else
			{
				throw new ArgumentException($"Error status code: {response.StatusCode}");
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
			var updateReviewRequest = new UpdateReviewRequest
			{
				Id = viewModel.ReviewId,
				Rating = viewModel.Rating,
				Text = Text.Text
			};

			var response = await HttpHelper.PostAsJsonAsync("http://10.0.2.2:8300/review/Update", updateReviewRequest);
			if (response.IsSuccessStatusCode) 
			{
				await DisplayAlert("Success", "Review edited successfully!", "OK");
				await Navigation.PopAsync();
            }
			else
			{
                throw new ArgumentException($"Error status code: {response.StatusCode}");
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