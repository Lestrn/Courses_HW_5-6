
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Courses_HW_5_6_Part_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DatabaseHelper dbHelper = new DatabaseHelper(DatabaseHelper.GetConnectionString());    
            Console.WriteLine("Sql Command");
            var ordersSqlCommand = dbHelper.GetOrdersDuringThisYearSqlCommand();
            PrintResult(ordersSqlCommand);
            Console.WriteLine("Dataset");
            var orderDataset =  dbHelper.GetOrdersDuringThisYearDataSet();
            PrintResult(orderDataset);
            dbHelper.AddOrder(28, DateOnly.Parse("05.05.2023"), 5);
            dbHelper.UpdateOrder(25, DateOnly.Parse("05.05.2023"), 4);
            dbHelper.DeleteOrder(29);
        }
        public static void PrintResult((List<string> orders, List<string> analysis) ordersSqlCommand)
        {
            if (ordersSqlCommand.orders != null || ordersSqlCommand.analysis != null)
            {
                for (int i = 0; i < ordersSqlCommand.orders.Count; i++)
                {
                    Console.WriteLine($"Order id = {ordersSqlCommand.orders[i]}, {ordersSqlCommand.analysis[i]}");
                }
            }
        }
    }
}