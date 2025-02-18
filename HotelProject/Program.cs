using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            HotelDatabase hotel = new HotelDatabase(connectionString);
            string command = Console.ReadLine();
            switch (command)
            {
                case "log":
                    string text = Console.ReadLine();
                    LogForExcel logForExcel = new LogForExcel();
                    logForExcel.WriteToWorkbook(text);
                    break;

                case "create":
                    hotel.CreateDatabaseAndTables();
                    break;

                case "insert":
                    hotel.InsertIntoTables(connectionString);
                    break;

                default:
                    Console.WriteLine("Invalid command! Try again!");
                    break;
            }
        }
    }
}
