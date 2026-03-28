using AutoParser.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoParser.Service
{
    public static class DatabaseService
    {
        private static string connectionString = "Data Source=autoservice.db";

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(connectionString);
        }
        public static void Initialize()
        {
            using var connection = GetConnection();
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            @"
    CREATE TABLE IF NOT EXISTS Cars (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Brand TEXT,
        Model TEXT,
        VIN TEXT,
        LicensePlate TEXT
    );

    CREATE TABLE IF NOT EXISTS ServiceTypes (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Name TEXT
    );

    CREATE TABLE IF NOT EXISTS Parts (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Name TEXT,
        PartNumber TEXT,
        Price REAL
    );

    CREATE TABLE IF NOT EXISTS ServiceOrders (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        CarId INTEGER,
        ServiceTypeId INTEGER,
        Date TEXT,
        Total REAL
    );

    CREATE TABLE IF NOT EXISTS ServiceOrderParts (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        ServiceOrderId INTEGER,
        PartId INTEGER,
        Quantity INTEGER,
        Price REAL
    );
    CREATE TABLE IF NOT EXISTS ServiceTypeParts (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        ServiceTypeId INTEGER,
        PartId INTEGER,
        Quantity INTEGER
    );
    ";
            command.ExecuteNonQuery();
            SeedData(connection);
        }
        private static void SeedData(SqliteConnection connection)
        {
            var check = connection.CreateCommand();
            check.CommandText = "SELECT COUNT(*) FROM Cars";

            long count = (long)check.ExecuteScalar();

            if (count > 0)
                return;

            var command = connection.CreateCommand();

            command.CommandText =
            @"
    INSERT INTO Cars (Brand, Model, VIN, LicensePlate) VALUES
    ('Toyota', 'Camry', 'JTNB11HK0K3000001', 'AB1234'),
    ('Toyota', 'Corolla', 'JTDBR32E720123456', 'CD5678'),
    ('Toyota', 'RAV4', 'JTMZFREV10D012345', 'EF9012'),
    ('BMW', 'X5', 'WBAKS410400C12345', 'GH3456'),
    ('Audi', 'A4', 'WAUZZZF48KA123456', 'IJ7890');

    INSERT INTO ServiceTypes (Name) VALUES
    ('Замена масла'),
    ('Замена колес'),
    ('Базовое ТО'),
    ('Расширенное ТО');

    INSERT INTO Parts (Name, PartNumber, Price) VALUES
    ('Масло моторное 5W30 4L', 'OIL-5W30', 45),
    ('Масляный фильтр', 'FILTER-OIL', 12),
    ('Воздушный фильтр', 'FILTER-AIR', 18),
    ('Салонный фильтр', 'FILTER-CABIN', 15),
    ('Свеча зажигания', 'SPARK-01', 10),
    ('Тормозные колодки передние', 'BRAKE-FRONT', 65),
    ('Тормозные колодки задние', 'BRAKE-REAR', 55);

    INSERT INTO ServiceTypeParts (ServiceTypeId, PartId, Quantity) VALUES
    (1,1,1),
    (1,2,1),
    (3,1,1),
    (3,2,1),
    (3,3,1),
    (4,1,1),
    (4,2,1),
    (4,3,1),
    (4,4,1),
    (4,5,4);
    ";

            command.ExecuteNonQuery();
        }
       
    }
}
