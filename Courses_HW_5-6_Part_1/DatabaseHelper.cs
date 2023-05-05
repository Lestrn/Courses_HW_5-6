using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
//В заданиях предполагается использование базы данных, созданной для выполнения заданий к лекции 3.
//1.      Используя SqlCommand и SqlDataReader, вывести на консоль все заказы (Orders) за последний год.
//2.      Выполнить условие задания 1, но с использованием SqlDataAdapter и DataSet
//3.      Выполнить условие задания 1, но с использованием EF Core
//4.      Используя SqlCommand создать новую запись в таблице Orders
//5.      Используя SqlCommand обновить одну из записей в таблице Orders
//6.      Используя SqlCommand удалить одну из записей в таблице Orders
//7.      Выполнить задачи 4-6, но с использованием EF Core

namespace Courses_HW_5_6_Part_1
{
    public class DatabaseHelper
    {
        private const string ORDERSDURINGYEARQUERY = "SELECT ord_id, an_name FROM Orders INNER JOIN Analysis ON (Orders.ord_an = Analysis.an_id) Where ord_datetime BETWEEN DATEADD(Year,-1,GetDate()) AND GetDate();";
        private const string ADDORDERQUERY = "INSERT INTO Orders (ord_id, ord_datetime, ord_an) VALUES (@ordId, @ordDatetime, @ordAn)"; // 0 = ord_id, 1 = ord_datetime, 2 = ord_an
        private const string UPDATEORDERQUERY = "UPDATE Orders SET ord_datetime = @ordDatetime, ord_an = @ordAn WHERE ord_id = @ordId";
        private const string DELETEORDERQUERY = "DELETE FROM Orders WHERE ord_id = @ordId";
        private SqlConnection _connection;
        public DatabaseHelper(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }
        public static string? GetConnectionString()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName);
            builder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();
            return config.GetConnectionString("DefaultConnection");
        }
        public (List<int>? orders, List<string>? analysis) GetOrdersDuringThisYearSqlCommand()
        {
            _connection.Open();
            SqlCommand sqlCommand = new SqlCommand(ORDERSDURINGYEARQUERY, _connection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (!sqlDataReader.HasRows)
            {
                return (null, null);
            }
            List<int> orders = new List<int>(5);
            List<string> analysis = new List<string>(5);
            while (sqlDataReader.Read())
            {
                orders.Add(sqlDataReader.GetInt32(0));
                analysis.Add(sqlDataReader.GetString(1));
            }
            _connection.Close();
            return (orders, analysis);
        }
        public (List<int>? orders, List<string>? analysis) GetOrdersDuringThisYearDataSet()
        {
            SqlDataAdapter adapter = new SqlDataAdapter(ORDERSDURINGYEARQUERY, _connection);
            DataSet ds = new DataSet();
            try
            {
                adapter.Fill(ds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (null, null);
            }
            List<int> orders = new List<int>(5);
            List<string> analysis = new List<string>(5);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                orders.Add(Convert.ToInt32(ds.Tables[0].Rows[i]["ord_id"]));
                analysis.Add(Convert.ToString(ds.Tables[0].Rows[i]["an_name"]));
            }
            return (orders, analysis);          
        }
        public bool AddOrder(int ordId, DateOnly dateTime, int ordAn)
        {
            return GeneralCommandExecuter(ADDORDERQUERY, ordId, dateTime, ordAn);
        }
        public bool UpdateOrder(int ordId, DateOnly dateTime, int ordAn)
        {
           return GeneralCommandExecuter(UPDATEORDERQUERY, ordId, dateTime, ordAn);         
        }
        public bool DeleteOrder(int ordId)
        {
            return GeneralCommandExecuter(DELETEORDERQUERY, ordId, isDelete:true);
        }
        private bool GeneralCommandExecuter(string query, int ordId, DateOnly dateTime = default, int ordAn = default, bool isDelete = false)
        {
            _connection.Open();
            using (SqlCommand sqlCommand = new SqlCommand(query, _connection))
            {
                sqlCommand.Parameters.Add(new SqlParameter("@ordId", ordId));
                if (!isDelete)
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@ordDatetime", dateTime));
                    sqlCommand.Parameters.Add(new SqlParameter("@ordAn", ordAn));
                }
                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _connection.Close();
                    return false;
                }
                _connection.Close();
            }
            return true;
        }

    }
}
