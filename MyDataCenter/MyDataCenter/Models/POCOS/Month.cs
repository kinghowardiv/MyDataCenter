using System.Collections.Generic;

namespace MyDataCenter.Models.POCOS
{
    public class Month
    {
        public double TotalPay { get; set; }
        public double Rent { get; set; }
        public string Name { get; set; }
        public double Utilities { get; set; }
        public int Id { get; set; }
        public List<Expense> RequiredExpenses { get; set; }
        public List<Expense> MonthlyExpenses { get; set; }
        public List<Expense> LuxuryExpenses { get; set; }  
    }
}