using OrderApi.Models.Responses;
using OrderBLL.Models;

namespace OrderApi.Mappers
{
    public static class OrderMapper
    {
        public static UserOrderResponse Map(RentOrder order)
        {
            return new UserOrderResponse
            {
                CarId = order.RentInfo.CarId,
            };
        }

        public static IEnumerable<UserOrderResponse> Map(IEnumerable<RentOrder> orders) 
        {
            return orders.Select(Map);
        }
    }
}
