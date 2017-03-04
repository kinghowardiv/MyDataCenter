using MyDataCenter.Models.POCOS;
using System.Collections.Generic;
using System;

namespace MyDataCenter.Business
{
    public interface IBudgetStatisicsCalculator
    {
        BudgetStatistics CalculateBudgetStatistics(List<double> luxuryExpenseList, List<double> requiredExpenseList,
            List<double> monthlyExpenseList, BudgetStatistics monthBudgetStats);
    }

    public class MonthlyBudgetStatisticsCalculator : IBudgetStatisicsCalculator
    {
        public BudgetStatistics CalculateBudgetStatistics(List<double> luxuryExpenseList,
            List<double> requiredExpenseList, List<double> monthlyExpenseList, BudgetStatistics monthBudgetStats)
        {
            monthBudgetStats.RequiredTotalSpent = CalculateRequiredSpent(requiredExpenseList);
            monthBudgetStats.MoneyLeftoverToSpendOnLuxury = CalculateMoneyLeftoverToSpendOnLuxury(monthBudgetStats);
            monthBudgetStats.MonthlyTotalSpent = CalculateMonthlyTotal(monthlyExpenseList);
            monthBudgetStats.LuxuryTotalSpent = CalculateLuxuryTotal(luxuryExpenseList);
            monthBudgetStats.TotalSpentPerMonth = CalculateTotalSpentPerMonth(monthBudgetStats);
            monthBudgetStats.TotalRemaining = CalculateTotalRemaining(monthBudgetStats);

            return monthBudgetStats;
        }

        private double CalculateRequiredSpent(List<double> requiredExpenseList)
        {
            var requiredSpent = 0.0;

            requiredExpenseList.ForEach(x => requiredSpent = x + requiredSpent);

            return requiredSpent;
        }

        private double CalculateMoneyLeftoverToSpendOnLuxury(BudgetStatistics monthBudgetStats)
        {
            return monthBudgetStats.TotalPay - monthBudgetStats.RequiredTotalSpent;
        }

        private double CalculateMonthlyTotal(List<double> monthlyExpenseList)
        {
            var monthlySpent = 0.0;

            monthlyExpenseList.ForEach(x => monthlySpent = x + monthlySpent);

            return monthlySpent;
        }


        private double CalculateLuxuryTotal(List<double> luxuryExpenseList)
        {
            var luxurySpent = 0.0;

            luxuryExpenseList.ForEach(x => luxurySpent = x + luxurySpent);

            return luxurySpent;
        }

        private double CalculateTotalSpentPerMonth(BudgetStatistics monthBudgetStats)
        {
            return monthBudgetStats.RequiredTotalSpent + monthBudgetStats.LuxuryTotalSpent +
                     monthBudgetStats.MonthlyTotalSpent;
        }

        private double CalculateTotalRemaining(BudgetStatistics monthBudgetStats)
        {
            return monthBudgetStats.TotalPay -
                   (monthBudgetStats.RequiredTotalSpent + monthBudgetStats.LuxuryTotalSpent +
                    monthBudgetStats.MonthlyTotalSpent);
        }
    }
}