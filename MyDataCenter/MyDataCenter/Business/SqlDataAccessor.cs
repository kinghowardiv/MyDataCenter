using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using MyDataCenter.Models.POCOS;
using System.IO;

namespace MyDataCenter.Business
{
    public interface ISqlDataAccessor
    {
        Month GetSingleMonthInfo(int currentMonth, int year);
        List<Expense> GetMonthlyExpenses(int currentMonth, int year);
        // void Update();
        void UpdateMonthlyInfo(Month monthInfo, int month, int year);
    }

    public class SqlDataAccessor : ISqlDataAccessor
    {        
        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection("user id=HOWIE-PC\\HOWIE;" +
                                       "password=Rusty123;server=HOWIE-PC;" +
                                       "Trusted_Connection=yes;" +
                                       "database=MyDataCenter; " +
                                       "connection timeout=30");
        }

        public Month GetSingleMonthInfo(int currentMonth, int year)
        {
            var myConnection = GetSqlConnection();

            myConnection.Open();
            var month = new Month();

            try
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("SELECT * FROM Month WHERE Id='" + currentMonth + "_" + year + "'",
                                                         myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {                   
                    month.TotalPay = Convert.ToDouble(myReader["TotalPay"]);
                    month.Rent = Convert.ToDouble(myReader["Rent"]);
                    month.Utilities = Convert.ToDouble(myReader["Utilities"]);
                    month.Name = myReader["Name"].ToString();
                }

                myConnection.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return month;
        }

        public List<Expense> GetMonthlyExpenses(int currentMonth, int year)
        {
            var myConnection = GetSqlConnection();

            myConnection.Open();
            var expenses = new List<Expense>();

            try
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("SELECT * FROM Expenses WHERE MonthId='" + currentMonth + "_" + year + "'",
                                                         myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var expense = new Expense();
                    expense.Name = myReader["Name"].ToString();
                    expense.Price = Convert.ToDouble(myReader["Price"]);
                    expense.Type = myReader["Type"].ToString();

                    expenses.Add(expense);
                }

                myConnection.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return expenses;
        }

        public void UpdateMonthlyInfo(Month monthInfo, int month, int year)
        {
            var myConnection = GetSqlConnection();

            myConnection.Open();

            SqlCommand myCommand = new SqlCommand();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.Text;

            myCommand.CommandText = "insert into Month(Id, TotalPay, Rent, Utilities, Name) values('" + month + "_" + year + "'" + "'," + monthInfo.TotalPay + "," + monthInfo.Rent + "," + monthInfo.Utilities + ",'" + monthInfo.Name+ "')";
            myCommand.ExecuteNonQuery();

            myConnection.Close();
        }

        public void Update()
        {
            var myConnection = GetSqlConnection();

            myConnection.Open();

            try
            {
                var namesList = ReadFile("C:\\Code\\febExpenseNames.txt");
                var priceList = ReadFile("C:\\Code\\febExpensePrices.txt");
                var counter = 0;

                foreach(var name in namesList)
                {
                    SqlCommand myCommand = new SqlCommand();
                    myCommand.Connection = myConnection;
                    myCommand.CommandType = CommandType.Text;

                    myCommand.CommandText = "insert into Expenses(Name, Price, Type, MonthId, Id) Values('" + name + "'," + priceList[counter] + ", 'Luxury', '2_2017'," + (counter + 4) + ")";
                    myCommand.ExecuteNonQuery();
                    counter++;
                }

                myConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private List<string> ReadFile(string fileName)
        {
            var info = new List<string>();
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;

                // Read and display lines from the file until 
                // the end of the file is reached. 
                var i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    info.Add(line);
                }
            }

            return info;
        }
    }
}