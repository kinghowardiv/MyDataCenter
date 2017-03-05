
namespace MyDataCenter.Models.POCOS
{
    public class BudgetStatistics
    {
        public double RequiredTotalSpent { get; set; }    
        public double MoneyLeftoverToSpendOnLuxury { get; set; }
        public double MonthlyTotalSpent { get; set; }
        public double LuxuryTotalSpent { get; set; }
        public double TotalSpentPerMonth { get; set; }
        public double TotalRemaining { get; set; }
    }
}