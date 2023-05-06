using Courses_HW_5_6_Part_2.Database;
using Courses_HW_5_6_Part_2.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Courses_HW_5_6_Part_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (DatabaseHelper dbHelper = new DatabaseHelper(DatabaseHelper.GetConnectionString()))
            {
                try
                {
                    dbHelper.InitializeDbWithRandomValues();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                var orders = dbHelper.GetOrdersDuringYear();
                foreach (var order in orders)
                {
                    Console.WriteLine($"Order date {order.Date}  {order.Analyis.Name}");
                }
                try
                {
                    dbHelper.AddOrder(new DateOnly(2023, 5, 12), dbHelper.GetAnalysById(4));
                    dbHelper.UpdateOrder(4, new DateOnly(2022, 4, 10), dbHelper.GetAnalysById(3));
                    dbHelper.DeleteOrder(15);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}