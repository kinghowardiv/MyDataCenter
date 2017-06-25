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
        List<Month> GetAllMonthsInfo();
        void CreateMonthInfo(Month monthInfo, int month, int year);
        void DeleteMonthInfo(string monthId);
        void UpdateMonthlyInfo(Month monthInfo, int month, int year);
        void UpdateExpenseInfo(Expense expense, int month, int year);
        void CreateExpense(Expense expense, int month, int year);
        void DeleteExpense(int expenseId);
        //  void Update();
    }

    public class SqlDataAccessor : ISqlDataAccessor
    {        
        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(@"user id=MicrosoftAccount\hapetersiv@gmail.com;" +
                                       @"password=Rusty123;server=DESKTOP-HG5PT58\SQLEXPRESS;" +
                                       "Trusted_Connection=yes;" +
                                       "database=MyDataCenter; " +
                                       "connection timeout=30");
        }

        public Month GetSingleMonthInfo(int currentMonth, int year)
        {
            var myConnection = GetSqlConnection();
            var month = new Month();

            myConnection.Open();

            try
            {
                SqlDataReader myReader = null;
                var myCommand = new SqlCommand("SELECT * FROM Month WHERE Id='" + currentMonth + "_" + year + "'",
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

        public List<Month> GetAllMonthsInfo()
        {
            var myConnection = GetSqlConnection();
            var months = new List<Month>();

            myConnection.Open();

            try
            {
                SqlDataReader myReader = null;
                var myCommand = new SqlCommand("SELECT * FROM Month",
                                                         myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var month = new Month();

                    month.TotalPay = Convert.ToDouble(myReader["TotalPay"]);
                    month.Rent = Convert.ToDouble(myReader["Rent"]);
                    month.Utilities = Convert.ToDouble(myReader["Utilities"]);
                    month.Name = myReader["Name"].ToString();

                    months.Add(month);
                }

                myConnection.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return months;
        }


        public void DeleteMonthInfo(string monthId)
        {
            var myConnection = GetSqlConnection();

            myConnection.Open();

            var myCommand = new SqlCommand();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.Text;

            myCommand.CommandText = "DELETE FROM Month WHERE Id=" + monthId;
            myCommand.ExecuteNonQuery();

            myConnection.Close();
        }

        public void CreateMonthInfo(Month monthInfo, int month, int year)
        {
            var myConnection = GetSqlConnection();

            myConnection.Open();

            var myCommand = new SqlCommand();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.Text;

            myCommand.CommandText = "INSERT INTO Month(Id, TotalPay, Rent, Utilities, Name) " +
               "VALUES('" + month + "_" + year + "'," + monthInfo.TotalPay + "," + monthInfo.Rent + "," + monthInfo.Utilities + ",'" + monthInfo.Name + "')";
            myCommand.ExecuteNonQuery();

            myConnection.Close();
        }

        public List<Expense> GetMonthlyExpenses(int currentMonth, int year)
        {
            var myConnection = GetSqlConnection();

            myConnection.Open();
            var expenses = new List<Expense>();

            try
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("SELECT * FROM Expense WHERE MonthId='" + currentMonth + "_" + year + "'",
                                                         myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var expense = new Expense();
                    expense.Name = myReader["Name"].ToString();
                    expense.Price = Convert.ToDouble(myReader["Price"]);
                    expense.Type = myReader["Type"].ToString();
                    expense.Id = Convert.ToInt32(myReader["Id"]);
                    expense.MonthId = myReader["MonthId"].ToString();

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

            var myCommand = new SqlCommand();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.Text;

            myCommand.CommandText = "INSERT INTO Month(Id, TotalPay, Rent, Utilities, Name) " +
               "VALUES('" + month + "_" + year + "'," + monthInfo.TotalPay + "," + monthInfo.Rent + "," + monthInfo.Utilities + ",'" + monthInfo.Name+ "')";
            myCommand.ExecuteNonQuery();

            myConnection.Close();
        }

        public void UpdateExpenseInfo(Expense expense, int month, int year)
        {
            var myConnection = GetSqlConnection();

            myConnection.Open();

            var myCommand = new SqlCommand();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.Text;

            myCommand.CommandText = "UPDATE Expense SET Price=" + expense.Price + ", Name='" +  expense.Name + "', Type='" + expense.Type + "' WHERE Id =" + expense.Id;
            myCommand.ExecuteNonQuery();

            myConnection.Close();
        }

        public void DeleteExpense(int expenseId)
        {
            var myConnection = GetSqlConnection();

            myConnection.Open();

            var myCommand = new SqlCommand();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.Text;

            myCommand.CommandText = "DELETE FROM Expense WHERE Id=" + expenseId;
            myCommand.ExecuteNonQuery();

            myConnection.Close();
        }

        public void CreateExpense(Expense expense, int month, int year)
        {
            var myConnection = GetSqlConnection();

            myConnection.Open();

            var myCommand = new SqlCommand();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.Text;

            expense.MonthId = month + "_" + year;

            myCommand.CommandText = "INSERT INTO Expense (Id, MonthId, Name, Price, Type) " +
               "VALUES(" + expense.Id + ",'" + expense.MonthId + "','" + expense.Name + "'," + expense.Price + ",'" + expense.Type + "')"; 
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
                    name.Trim();
                    priceList[counter].Trim();
                    var priceAsInt = Convert.ToDouble(priceList[counter]);
                    var myCommand = new SqlCommand();
                    myCommand.Connection = myConnection;
                    myCommand.CommandType = CommandType.Text;

                    myCommand.CommandText = "insert into Expense(Name, Price, Type, MonthId, Id) Values('" + name + "'," + priceList[counter] + ", 'Luxury', '5_2017'," + (counter + 7) + ")";
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