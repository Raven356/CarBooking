using OrderDAL.Models;

namespace OrderDAL.Context
{
    public class OrderContextSeeder
    {
        public static void SeedData(OrderContext context)
        {
            if (!context.RentOrder.Any())
            {
                context.Add(new RentOrderDTO
                {
                    IsAcepted = true,
                    RentInfoId = 1,
                    RentInfoDTO = new RentInfoDTO
                    {
                        CarId = 4,
                        RentBy = 1,
                        RentFromUTC = DateTime.UtcNow.AddDays(-2),
                        RentToUTC = DateTime.UtcNow.AddDays(-1),
                        RentFinished = DateTime.UtcNow.AddDays(-1)
                    }
                });

                context.SaveChanges();
            }
        }
    }
}
