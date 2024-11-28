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
                StartDate = model.RentInfo.RentFromUTC.ToShortDateString(),
                EndDate = model.RentInfo.RentToUTC.ToShortDateString(),
                Name = $"#{model.Id}"
            };
        }

        public static IEnumerable<Order> Map(IEnumerable<OrderModel> orders) 
        {
            return orders.Select(Map);
        }
    }
}
