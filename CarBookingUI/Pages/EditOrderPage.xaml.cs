using CarBookingUI.Helpers;
using CarBookingUI.Models.Requests.OrderRequests;
using CarBookingUI.Models.Responses.OrderResponse;
using CarBookingUI.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace CarBookingUI.Pages;

public partial class EditOrderPage : ContentPage
{
	private EditOrderPageViewModel viewModel;

	public EditOrderPage(int orderId)
	{
		InitializeComponent();
		viewModel = new EditOrderPageViewModel()
		{
			OrderId = orderId
		};

        BindingContext = viewModel;
	}

    protected override async void OnAppearing()
	{
		base.OnAppearing();

		try
		{
            var orderResponse = await HttpHelper.GetAsync($"http://10.0.2.2:8300/order/GetOrderById?orderId={viewModel.OrderId}");

            if (orderResponse.IsSuccessStatusCode)
            {
                var responseContent = await orderResponse.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<OrderCarResponse>(responseContent);

                viewModel.DateFrom = order.RentFromUTC.Date;
                viewModel.DateTo = order.RentToUTC.Date;
                viewModel.TimeFrom = TimeOnly.FromDateTime(order.RentFromUTC);
                viewModel.TimeTo = TimeOnly.FromDateTime(order.RentToUTC);
                viewModel.Name = $"Chosed car: {order.CarPlate}";
                viewModel.Description = "Here you can choose new time for your order.";
            }
            else
            {
                await DisplayAlert("Error", $"Something went wrong: {orderResponse.StatusCode}!", "OK");
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
            var selectedFromDateTime = DateFrom.Date.ToUniversalTime().Add(TimeFrom.Time);
            DateTime selectedDateToUtc = DateTo.Date.ToUniversalTime();

            TimeSpan selectedTimeTo = TimeTo.Time;

            // Об’єднуємо дату і час
            DateTime selectedDateTimeTo = selectedDateToUtc.Add(selectedTimeTo);

            var request = new EditOrderRequest
            {
                Id = viewModel.OrderId,
                DateFrom = selectedFromDateTime,
                DateTo = selectedDateTimeTo,
            };

            if (ValidateToDate(selectedFromDateTime, selectedDateTimeTo))
            {
                var response = await HttpHelper.PostAsJsonAsync("http://10.0.2.2:8300/order/EditOrder", request);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Order Edited", "Time was changed!", "OK");
                    await Navigation.PushAsync(new HistoryOrderDetailsPage(viewModel.OrderId));
                }
                else
                {
                    throw new ArgumentException("Order request had errors!");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Something went wrong: {ex.Message}!", "OK");
        }
    }

    private bool ValidateToDate(DateTime selectedDateTimeFrom, DateTime selectedDateTimeTo)
    {
        // Отримуємо поточний час в UTC
        DateTime currentUtcNow = DateTime.UtcNow;

        var stringBuilder = new StringBuilder();

        // Перевіряємо, чи обрана дата пізніше за поточну
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