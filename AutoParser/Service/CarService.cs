using AutoParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoParser.Service
{
    public class CarService
    {
        public static List<Car> GetCars()
        {
            List<Car> cars = new List<Car>();

            using var connection = DatabaseService.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Cars";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                cars.Add(new Car
                {
                    Id = reader.GetInt32(0),
                    Brand = reader.GetString(1),
                    Model = reader.GetString(2),
                    VIN = reader.GetString(3),
                    LicensePlate = reader.GetString(4)
                });
            }

            return cars;
        }
        public static List<ServiceHistory> GetHistory(int carId)
        {
            List<ServiceHistory> list = new List<ServiceHistory>();

            using var connection = DatabaseService.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            @"
    SELECT 
        ServiceOrders.Date,
        ServiceTypes.Name,
        ServiceOrders.Total
    FROM ServiceOrders
    JOIN ServiceTypes ON ServiceOrders.ServiceTypeId = ServiceTypes.Id
    WHERE ServiceOrders.CarId = @carId
    ORDER BY Date DESC
    ";

            command.Parameters.AddWithValue("@carId", carId);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new ServiceHistory
                {
                    Date = reader.GetString(0),
                    ServiceName = reader.GetString(1),
                    Total = reader.GetDouble(2)
                });
            }

            return list;
        }
    }
}
