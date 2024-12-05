using CarBookingDAL.Models;

namespace CarBookingDAL.Context
{
    public class CarBookingContextSeeder
    {
        public static void SeedData(CarCatalogContext context)
        {
            //context.Database.Migrate();

            // Check if data already exists

            if (!context.CarTypeDTOs.Any()) 
            {
                context.CarTypeDTOs.AddRange(
                [
                    new CarTypeDTO() { Type = "type1" },
                    new CarTypeDTO() { Type = "type2" }
                ]);
            }
            context.SaveChanges();
            var carTypeDTO = context.CarTypeDTOs.First(type => type.Id == 1);
            var carType2 = context.CarTypeDTOs.First(type => type.Id == 2);

            if (!context.CarModelDTOs.Any())
            {
                context.CarModelDTOs.AddRange(
                [
                    new CarModelDTO { Model = "model1" },
                    new CarModelDTO { Model = "model2" }
                ]); 
            }
            context.SaveChanges();
            var carModelDTO = context.CarModelDTOs.First(model => model.Id == 1);
            var carModel2 = context.CarModelDTOs.First(model => model.Id == 2);

            string carPath1 = @"../src/CarCatalogDAL/DebugImages/car1.jpg";
            string carPath2 = @"../src/CarCatalogDAL/DebugImages/car2.jpg";
            string carPath3 = @"../src/CarCatalogDAL/DebugImages/car3.jpg";
            string carPath4 = @"../src/CarCatalogDAL/DebugImages/car4.jpg";
            string carPath5 = @"../src/CarCatalogDAL/DebugImages/car5.jpg";

            if (!context.CarDTOs.Any())
            {
                context.CarDTOs.AddRange(
                [
                    new CarDTO { CarPlate = "plate1", CarType = carTypeDTO, RentPrice = 2000, Model = carModelDTO, Image = GetImageAsByteArray(carPath1) },
                    new CarDTO { CarPlate = "plate2", CarType = carTypeDTO, RentPrice = 2000, Model = carModelDTO, Image = GetImageAsByteArray(carPath2) },
                    new CarDTO { CarPlate = "plate3", CarType = carTypeDTO, RentPrice = 2000, Model = carModelDTO, Image = GetImageAsByteArray(carPath3) },
                    new CarDTO { CarPlate = "plate4", CarType = carTypeDTO, RentPrice = 2000, Model = carModelDTO, Image = GetImageAsByteArray(carPath4) },
                    new CarDTO { CarPlate = "plate5", CarType = carType2, RentPrice = 1000, Model = carModelDTO, Image = GetImageAsByteArray(carPath5) },
                    new CarDTO { CarPlate = "plate6", CarType = carTypeDTO, RentPrice = 3000, Model = carModel2, Image = GetImageAsByteArray(carPath1) },
                ]);

                context.SaveChanges();
            }
        }

        private static byte[] GetImageAsByteArray(string filePath)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found.", filePath);

            return File.ReadAllBytes(filePath);
        }
    }
}
