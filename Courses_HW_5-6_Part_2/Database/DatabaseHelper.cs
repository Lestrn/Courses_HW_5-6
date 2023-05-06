using Courses_HW_5_6_Part_2.Database;
using Courses_HW_5_6_Part_2.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//В заданиях предполагается использование базы данных, созданной для выполнения заданий к лекции 3.
//1.      Используя SqlCommand и SqlDataReader, вывести на консоль все заказы (Orders) за последний год.
//2.      Выполнить условие задания 1, но с использованием SqlDataAdapter и DataSet
//3.      Выполнить условие задания 1, но с использованием EF Core
//4.      Используя SqlCommand создать новую запись в таблице Orders
//5.      Используя SqlCommand обновить одну из записей в таблице Orders
//6.      Используя SqlCommand удалить одну из записей в таблице Orders
//7.      Выполнить задачи 4-6, но с использованием EF Core
namespace Courses_HW_5_6_Part_2
{
    public class DatabaseHelper : IDisposable
    {
        private HomeworkDbContext _dbContext;
        public DatabaseHelper(string dbConnection)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HomeworkDbContext>();
            var options = optionsBuilder
                   .UseSqlServer(dbConnection)
                   .Options;
            _dbContext = new HomeworkDbContext(options);
        }
        public static string? GetConnectionString()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName);
            builder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();
            return config.GetConnectionString("DefaultConnection");
        }
        public void InitializeDbWithRandomValues()
        {
            bool existsGroup = _dbContext.Groups.Any();
            bool existsAnalyis = _dbContext.Analysis.Any();
            bool existsOrder = _dbContext.Orders.Any();
            if (existsGroup || existsAnalyis || existsOrder)
            {
                throw new Exception("Cant initilize db since its already has values");
            }
            Random random = new Random();
            List<Groups> groups = RandGroups();
            List<Analysis> analyses = new List<Analysis>(10);
            for (int i = 0; i < 20; i++)
            {
                analyses.Add(new Analysis() { Cost = random.Next(50, 500), Name = $"Analyis {i}", Price = random.Next(50, 500), Group = groups[random.Next(0, 20)] });
            }
            List<Orders> orders = new List<Orders>(10);
            for (int i = 0; i < 20; i++)
            {
                orders.Add(new Orders() { Date = DateTime.Parse($"{random.Next(1, 30)}.{random.Next(1, 12)}.{random.Next(2019, 2024)}"), Analyis = analyses[random.Next(0, 20)] });
            }
            _dbContext.Groups.AddRange(groups);
            _dbContext.Analysis.AddRange(analyses);
            _dbContext.Orders.AddRange(orders);
            _dbContext.SaveChanges();
        }
        public List<Orders> GetOrdersDuringYear()
        {
            DateTime dateOneYearAgo = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, DateTime.Now.Day);
            return _dbContext.Orders
                .Where(order => order.Date < DateTime.Now && order.Date > dateOneYearAgo).Include(order => order.Analyis)
                .ToList();
        }
        public void AddOrder(DateOnly date, Analysis? analysis = null)
        {
            _dbContext.Orders.Add(new Orders() { Date = DateTime.Parse(date.ToString()), Analyis = AnalysIsValid(analysis) ? analysis : null });
            _dbContext.SaveChanges();
        }
        public void UpdateOrder(int id, DateOnly date, Analysis? analysis = null)
        {
            var order = _dbContext.Orders.FirstOrDefault(order => order.Id == id);
            if (order == null)
            {
                throw new ArgumentException("No order with such id was found");
            }
            if(AnalysIsValid(analysis))
            {
                order.Analyis = analysis;
            }
            order.Date = DateTime.Parse(date.ToString());
            _dbContext.Orders.Update(order);
            _dbContext.SaveChanges();
        }
        public void DeleteOrder(int id)
        {
            var order = _dbContext.Orders.FirstOrDefault(order => order.Id == id);
            if (order == null)
            {
                throw new ArgumentException("No order with such id was found");
            }
            _dbContext.Orders.Remove(order);
            _dbContext.SaveChanges();

        } 
        public void Dispose()
        {
            _dbContext.Dispose();
        }
        public Analysis? GetAnalysById(int id)
        {
            return _dbContext.Analysis.FirstOrDefault(analys => analys.Id == id);
        }
        private bool AnalysIsValid(Analysis? analys)
        {
            if (analys == null)
            {
                return false;
            }
            return _dbContext.Analysis.Any(analy => analy.Id == analys.Id);    
        }
        private List<Groups> RandGroups()
        {
            Random random = new Random();
            var groups = new List<Groups>(10);
            for (int i = 0; i < 20; i++)
            {
                groups.Add(new Groups() { Name = $"Group  {i}", Temp = random.Next(15, 40) });
            }
            return groups;
        }
    }
}
