using CarBookingUI.Helpers;
using CarBookingUI.Models.Requests.OrderRequests;
using CarBookingUI.Models.Responses.UserResponses;
using CarBookingUI.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace CarBookingUI.Pages;

public partial class OrderPage : ContentPage
{
    private int carId;

    public OrderPage(OrderPageViewModel viewModel)
	{
		InitializeComponent();

        carId = viewModel.CarId;
        BindingContext = viewModel;
    }

    private async void OnConfirmButtonClicked(object sender, EventArgs e)
    {
        try
        {
            var selectedFromDateTime = DateFrom.Date.ToUniversalTime().Add(TimeFrom.Time);
            DateTime selectedDateToUtc = DateTo.Date.ToUniversalTime();

            TimeSpan selectedTimeTo = TimeTo.Time;

            // �ᒺ����� ���� � ���
            DateTime selectedDateTimeTo = selectedDateToUtc.Add(selectedTimeTo);

            if (ValidateToDate(selectedFromDateTime, selectedDateTimeTo))
            {
                var userResponse = await HttpHelper.GetAsync($"http://10.0.2.2:8300/api/v1/User/GetUserIdByToken?token={await SecureStorage.GetAsync("auth_token")}");
                if (userResponse.IsSuccessStatusCode)
                {
                    var responseContent = await userResponse.Content.ReadAsStringAsync();
                    var userIdResponse = JsonConvert.DeserializeObject<UserIdResponse>(responseContent);

                    var request = new CreateOrderRequest
                    {
                        CarId = carId,
                        DateTo = selectedDateTimeTo,
                        UserId = userIdResponse.UserId,
                        DateFrom = selectedFromDateTime
                    };

                    var orderResponse = await HttpHelper.PostAsJsonAsync($"http://10.0.2.2:8300/order/CreateOrder", request);

                    if (orderResponse.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Order Created", "Your car has been booked!", "OK");
                        await Navigation.PushAsync(new MainPage());
                    }
                    else
                    {
                        throw new ArgumentException("Order request had errors!");
                    }
                }
            }
        }
        catch (Exception ex) 
        {
            await DisplayAlert("Order error", $"Something went wrong, error: {ex.Message}", "OK");
        }
    }

    private bool ValidateToDate(DateTime selectedDateTimeFrom, DateTime selectedDateTimeTo)
    {
        // �������� �������� ��� � UTC
        DateTime currentUtcNow = DateTime.UtcNow;

        var stringBuilder = new StringBuilder();

        // ����������, �� ������ ���� ����� �� �������
        if (selectedDateTimeFrom < currentUtcNow)
        {
            stringBuilder.AppendLine("DateFrom must be later or equeal current date!");
        }

        if (selectedDateTimeTo < selectedDateTimeFrom)
        {
            stringBuilder.AppendLine("Date to must be greater than date from!");
        }

        if (stringBuilder.Length > 0)
        {
            DisplayAlert("Invalid Request", stringBuilder.ToString(), "OK");
            return false;
        }

        return true;
    }
}