using AutoParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoParser.Service
{
    public class ServiceTypeService
    {
        public static List<ServiceType> GetTypes()
        {
            List<ServiceType> list = new List<ServiceType>();

            using var connection = DatabaseService.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM ServiceTypes";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new ServiceType
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }

            return list;
        }
    }
}
