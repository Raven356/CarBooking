using CarBookingUI.Models;
using CarBookingUI.Models.Responses.OrderResponse;

namespace CarBookingUI.Mappers
{
    internal static class OrderMapper
    {
        public static Order Map(OrderModel model)
        {
            return new Order
            {
                Id = model.Id,
                StartDate = model.RentInfo.RentFromUTC.ToLongDateString(),
                EndDate = model.RentInfo.RentToUTC.ToLongDateString(),
                Name = $"Order #{model.Id}, Accepted: {model.IsAcepted}"
            };
        }

        public static IEnumerable<Order> Map(IEnumerable<OrderModel> orders) 
        {
            return orders.Select(Map);
        }
    }
}
