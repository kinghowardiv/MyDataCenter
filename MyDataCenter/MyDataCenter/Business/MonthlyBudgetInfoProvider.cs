using MyDataCenter.Models.POCOS;
using MyDataCenter.Business;
using System.Collections.Generic;


namespace MyDataCenter.Business
{
    public interface IMonthlyBudgetInfoProvider
    {
        Month GetCurrentMonthInfo(int month, int year);
        void UpdateCurrentMonthInfo(int month, int year, Month monthInfo);
    }

    public class MonthlyBudgetInfoProvider : IMonthlyBudgetInfoProvider
    {
        private ISqlDataAccessor _sqlDataAccessor = new SqlDataAccessor();

        public Month GetCurrentMonthInfo(int month, int year)
        {
            var currentMonthInfo = _sqlDataAccessor.GetSingleMonthInfo(month, year);
            var allMontlyExpenses = _sqlDataAccessor.GetMonthlyExpenses(month, year);

            currentMonthInfo.RequiredExpenses = GetRequiredExpenses(allMontlyExpenses);
            currentMonthInfo.MonthlyExpenses = GetMonthlyExpenses(allMontlyExpenses);
            currentMonthInfo.LuxuryExpenses = GetLuxuryExpenses(allMontlyExpenses);

            return currentMonthInfo;
        }

        public void UpdateCurrentMonthInfo(int month, int year, Month monthInfo)
        {
            _sqlDataAccessor.UpdateMonthlyInfo(monthInfo, month, year);
        }

        private List<Expense> GetRequiredExpenses(List<Expense> allMonthlyExpenses)
        {
            var requiredExpenses = new List<Expense>();

            foreach (var expense in allMonthlyExpenses)
            {
                if (expense.Type == "Required")
                    requiredExpenses.Add(expense);
            }

            return requiredExpenses;
        }

        private List<Expense> GetMonthlyExpenses(List<Expense> allMonthlyExpenses)
        {
            var monthlyExpenses = new List<Expense>();

            foreach (var expense in allMonthlyExpenses)
            {
                if (expense.Type == "Monthly")
                    monthlyExpenses.Add(expense);
            }

            return monthlyExpenses;
        }

        private List<Expense> GetLuxuryExpenses(List<Expense> allMonthlyExpenses)
        {
            var luxuryExpenses = new List<Expense>();

            foreach (var expense in allMonthlyExpenses)
            {
                if (expense.Type == "Luxury")
                    luxuryExpenses.Add(expense);
            }

            return luxuryExpenses;
        }
    }
}