using MyDataCenter.Models.POCOS;
using System.Collections.Generic;
using System;

namespace MyDataCenter.Business
{
    public interface IBudgetStatisicsCalculator
    {
        void CalculateBudgetStatistics(Month currentMonthInfo);
    }

    public class MonthlyBudgetStatisticsCalculator : IBudgetStatisicsCalculator
    {
        public void CalculateBudgetStatistics(Month currentMonthInfo)
        {
            var monthBudgetStats = new BudgetStatistics();
            currentMonthInfo.BudgetStatistics = monthBudgetStats;

            monthBudgetStats.RequiredTotalSpent = CalculateRequiredSpent(currentMonthInfo.RequiredExpenses, currentMonthInfo);
            monthBudgetStats.MoneyLeftoverToSpendOnLuxury = CalculateMoneyLeftoverToSpendOnLuxury(currentMonthInfo);
            monthBudgetStats.MonthlyTotalSpent = CalculateMonthlyTotal(currentMonthInfo.MonthlyExpenses);
            monthBudgetStats.LuxuryTotalSpent = CalculateLuxuryTotal(currentMonthInfo.LuxuryExpenses);
            monthBudgetStats.TotalSpentPerMonth = CalculateTotalSpentPerMonth(monthBudgetStats);
            monthBudgetStats.TotalRemaining = CalculateTotalRemaining(monthBudgetStats, currentMonthInfo);
        }

        private double CalculateRequiredSpent(List<Expense> requiredExpenseList, Month currentMonthInfo)
        {
            var requiredSpent = 0.0;

            foreach (var expense in requiredExpenseList)
            {
                requiredSpent = expense.Price + requiredSpent;
            }

            requiredSpent = requiredSpent + currentMonthInfo.Rent + currentMonthInfo.Utilities;

            return requiredSpent;
        }

        private double CalculateMoneyLeftoverToSpendOnLuxury(Month currentMonthInfo)
        {
            return currentMonthInfo.TotalPay - currentMonthInfo.BudgetStatistics.RequiredTotalSpent;
        }

        private double CalculateMonthlyTotal(List<Expense> monthlyExpenseList)
        {
            var monthlySpent = 0.0;

            foreach (var expense in monthlyExpenseList)
            {
                monthlySpent = expense.Price + monthlySpent;
            }

            return monthlySpent;
        }


        private double CalculateLuxuryTotal(List<Expense> requiredExpenseList)
        {
            var luxurySpent = 0.0;

            foreach (var expense in requiredExpenseList)
            {
                luxurySpent = expense.Price + luxurySpent;
            }

            return luxurySpent;
        }

        private double CalculateTotalSpentPerMonth(BudgetStatistics monthBudgetStats)
        {
            return monthBudgetStats.RequiredTotalSpent + monthBudgetStats.LuxuryTotalSpent +
                     monthBudgetStats.MonthlyTotalSpent;
        }

        private double CalculateTotalRemaining(BudgetStatistics monthBudgetStats, Month currentMonthInfo)
        {
            return currentMonthInfo.TotalPay -
                   (monthBudgetStats.RequiredTotalSpent + monthBudgetStats.LuxuryTotalSpent +
                    monthBudgetStats.MonthlyTotalSpent);
        }
    }
}