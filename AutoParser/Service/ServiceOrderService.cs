using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoParser.Service
{
    public class ServiceOrderService
    {
        public static void CreateOrder(int carId, int typeId, double total)
        {
            using var connection = DatabaseService.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            @"
            INSERT INTO ServiceOrders (CarId, ServiceTypeId, Date, Total)
            VALUES (@carId, @typeId, @date, @total)
            ";

            command.Parameters.AddWithValue("@carId", carId);
            command.Parameters.AddWithValue("@typeId", typeId);
            command.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@total", total);

            command.ExecuteNonQuery();
        }
    }
}
