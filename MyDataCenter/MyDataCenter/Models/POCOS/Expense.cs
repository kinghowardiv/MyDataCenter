

namespace MyDataCenter.Models.POCOS
{
    public class Expense
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }

    public class MonthlyExpense : Expense
    {

    }

    public class LuxuryExpense : Expense
    {

    }

    public class RequiredExpense : Expense
    {

    }
}