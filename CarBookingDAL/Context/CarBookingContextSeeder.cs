using CarBookingDAL.Models;

namespace CarBookingDAL.Context
{
    public class CarBookingContextSeeder
    {
        public static void SeedData(CarBookingContext context)
        {
            //context.Database.Migrate();

            // Check if data already exists

            if (!context.CarTypeDTOs.Any()) 
            {
                context.CarTypeDTOs.AddRange(
                [
                    new CarTypeDTO() { Type = "type1" }
                ]);
            }
            context.SaveChanges();
            var carTypeDTO = context.CarTypeDTOs.First(type => type.Id == 1);

            if (!context.CarModelDTOs.Any())
            {
                context.CarModelDTOs.AddRange(
                [
                    new CarModelDTO { Model = "model1" }
                ]); 
            }
            context.SaveChanges();
            var carModelDTO = context.CarModelDTOs.First(model => model.Id == 1);

            if (!context.CarDTOs.Any())
            {
                context.CarDTOs.AddRange(
                [
                    new CarDTO { CarPlate = "plate1", CarType = carTypeDTO, RentPrice = 2000, Model = carModelDTO },
                    new CarDTO { CarPlate = "plate2", CarType = carTypeDTO, RentPrice = 2000, Model = carModelDTO },
                    new CarDTO { CarPlate = "plate3", CarType = carTypeDTO, RentPrice = 2000, Model = carModelDTO }
                ]);

                context.SaveChanges();
            }
        }
    }
}
