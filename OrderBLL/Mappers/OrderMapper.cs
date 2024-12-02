using OrderBLL.Models;
using OrderDAL.Models;

namespace OrderBLL.Mappers
{
    public static class OrderMapper
    {
        public static RentOrderDTO Map(RentOrder order)
        {
            return new RentOrderDTO
            {
                Id = order.Id,
                IsAcepted = order.IsAcepted,
                RentInfoDTO = Map(order.RentInfo),
                RentInfoId = order.RentInfo.Id
            };
        }

        public static RentOrder Map(RentOrderDTO rentOrder)
        {
            return new RentOrder
            {
                Id = rentOrder.Id,
                IsAcepted = rentOrder.IsAcepted,
                RentInfo = Map(rentOrder.RentInfoDTO)
            };
        }

        public static IEnumerable<RentOrder> Map(IEnumerable<RentOrderDTO> rentOrders)
        {
            return rentOrders.Select(Map);
        }

        public static RentInfoDTO Map(RentInfo info)
        {
            return new RentInfoDTO
            {
                Id = info.Id,
                CarId = info.CarId,
                RentBy = info.RentBy,
                RentFromUTC = info.RentFromUTC,
                RentToUTC = info.RentToUTC,
                RentFinished = info.RentFinished
            };
        }

        public static RentInfo Map(RentInfoDTO rentInfo)
        {
            return new RentInfo
            {
                Id = rentInfo.Id,
                CarId = rentInfo.CarId,
                RentBy = rentInfo.RentBy,
                RentFromUTC = rentInfo.RentFromUTC,
                RentToUTC = rentInfo.RentToUTC,
                RentFinished = rentInfo.RentFinished,
            };
        }
    }
}
