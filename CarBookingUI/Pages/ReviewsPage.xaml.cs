using CarBookingUI.Helpers;
using CarBookingUI.Models;
using CarBookingUI.Models.Responses.ReviewsResponses;
using CarBookingUI.ViewModels;
using Newtonsoft.Json;

namespace CarBookingUI.Pages;

public partial class ReviewsPage : ContentPage
{
	private readonly ReviewsPageViewModel viewModel;

	public ReviewsPage()
	{
		InitializeComponent();
		viewModel = new ReviewsPageViewModel();
		BindingContext = viewModel;
	}

    private async void OnReviewTapped(object sender, ItemTappedEventArgs e)
    {
        await Navigation.PushAsync(new UpdateReviewPage(((Review)e.Item).Id));
    }

    private async void OnMenuClicked(object sender, EventArgs e)
    {
        string action = await DisplayActionSheet("Choose an action", "Cancel", null, "Logout", "Main page", "Review");

        switch (action)
        {
            case "Logout":
                await DisplayAlert("Action", "You selected Logout", "OK");
                break;
            case "Main page":
                await Navigation.PushAsync(new MainPage());
                break;
            case "Review":
                await DisplayAlert("Action", "You selected Option 3", "OK");
                break;
            default:
                // Cancel or null case
                break;
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var reviewsResponse = await HttpHelper.GetAsync($"http://10.0.2.2:8300/review/GetByUserId?userId={await SecureStorage.GetAsync("userId")}");
            if (reviewsResponse.IsSuccessStatusCode)
            {
                var reviewContent = await reviewsResponse.Content.ReadAsStringAsync();
                var reviews = JsonConvert.DeserializeObject<ReviewsCollectionResponse>(reviewContent);

                viewModel.Reviews = new System.Collections.ObjectModel.ObservableCollection<Models.Review>(reviews.Reviews);
            }
            else
            {
                throw new ArgumentException($"Error, status code: {reviewsResponse.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Something went wrong: {ex.Message}!", "OK");
        }
    }
}