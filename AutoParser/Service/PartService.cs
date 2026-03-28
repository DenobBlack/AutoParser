using AutoParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoParser.Service
{
    public class PartService
    {
        public static List<Part> GetPartsForService(int serviceTypeId)
        {
            List<Part> list = new List<Part>();

            using var connection = DatabaseService.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            @"
            SELECT 
                Parts.Id,
                Parts.Name,
                ServiceTypeParts.Quantity,
                Parts.Price,
                Parts.PartNumber
            FROM ServiceTypeParts
            JOIN Parts ON Parts.Id = ServiceTypeParts.PartId
            WHERE ServiceTypeParts.ServiceTypeId = @typeId
            ";

            command.Parameters.AddWithValue("@typeId", serviceTypeId);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Part
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Quantity = reader.GetInt32(2),
                    Price = reader.GetDouble(3),
                    PartNumber = reader.GetString(4)
                });
            }

            return list;
        }
        public static List<Part> GetAllParts()
        {
            List<Part> list = new List<Part>();

            using var connection = DatabaseService.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            @"
    SELECT Id, Name, PartNumber, Price
    FROM Parts
    ";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Part
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    PartNumber = reader.GetString(2),
                    Price = reader.GetDouble(3)
                });
            }

            return list;
        }
        public static void AddPart(Part part)
        {
            using var connection = DatabaseService.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            @"
    INSERT INTO Parts (Name, PartNumber, Price)
    VALUES (@name,@number,@price)
    ";

            command.Parameters.AddWithValue("@name", part.Name);
            command.Parameters.AddWithValue("@number", part.PartNumber);
            command.Parameters.AddWithValue("@price", part.Price);

            command.ExecuteNonQuery();
        }
        public static void UpdatePart(Part part)
        {
            using var connection = DatabaseService.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            @"
    UPDATE Parts
    SET Name=@name,
        PartNumber=@number,
        Price=@price
    WHERE Id=@id
    ";

            command.Parameters.AddWithValue("@name", part.Name);
            command.Parameters.AddWithValue("@number", part.PartNumber);
            command.Parameters.AddWithValue("@price", part.Price);
            command.Parameters.AddWithValue("@id", part.Id);

            command.ExecuteNonQuery();
        }
        public static void DeletePart(int id)
        {
            using var connection = DatabaseService.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            "DELETE FROM Parts WHERE Id=@id";

            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }
        public static List<Part> SearchParts(string text)
        {
            List<Part> list = new List<Part>();

            using var connection = DatabaseService.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            @"
    SELECT Id, Name, Price, PartNumber
    FROM Parts
    WHERE Name LIKE @text
    OR PartNumber LIKE @text
    ";

            command.Parameters.AddWithValue("@text", "%" + text + "%");

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Part
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDouble(2),
                    PartNumber = reader.GetString(3),
                    Quantity = 1
                });
            }

            return list;
        }
    }
}
