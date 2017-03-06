using MyDataCenter.Models.POCOS;
using System.Collections.Generic;
using System.Linq;

namespace MyDataCenter.Business
{
    public interface IMonthlyBudgetInfoProvider
    {
        Month GetCurrentMonthInfo(int month, int year);
        void UpdateCurrentMonthInfo(int month, int year, Month monthInfo);
        void UpdateExpenseInfo(int month, int year, Expense expense);
        List<Expense> GetExpensesToUpdate(Month monthInfo, int[] expensesIds);
        void DeleteExpense(int[] expenseIds);
    }

    public class MonthlyBudgetInfoProvider : IMonthlyBudgetInfoProvider
    {
        private ISqlDataAccessor _sqlDataAccessor;
        private IMonthlyBudgetStatisicsCalculator _budgetStatsCalc;

        public MonthlyBudgetInfoProvider(ISqlDataAccessor sqlDataAccessor, IMonthlyBudgetStatisicsCalculator budgetStatsCalc)
        {
            _sqlDataAccessor = sqlDataAccessor;
            _budgetStatsCalc = budgetStatsCalc;
        }

        public Month GetCurrentMonthInfo(int month, int year)
        {
            var currentMonthInfo = _sqlDataAccessor.GetSingleMonthInfo(month, year);
            var allMontlyExpenses = _sqlDataAccessor.GetMonthlyExpenses(month, year);

            currentMonthInfo.RequiredExpenses = GetRequiredExpenses(allMontlyExpenses);
            currentMonthInfo.MonthlyExpenses = GetMonthlyExpenses(allMontlyExpenses);
            currentMonthInfo.LuxuryExpenses = GetLuxuryExpenses(allMontlyExpenses);

            _budgetStatsCalc.CalculateBudgetStatistics(currentMonthInfo);

            return currentMonthInfo;
        }

        public void UpdateCurrentMonthInfo(int month, int year, Month monthInfo)
        {
            _sqlDataAccessor.UpdateMonthlyInfo(monthInfo, month, year);
        }

        public void UpdateExpenseInfo(int month, int year, Expense expense)
        {
            _sqlDataAccessor.UpdateExpenseInfo(expense, month, year);
        }

        public List<Expense> GetExpensesToUpdate(Month monthInfo, int[] expensesIds)
        {
            var expenseList = new List<Expense>();

            foreach (var id in expensesIds)
            {
                var expense = new Expense();

                if (IsLuxuryExpense(monthInfo.LuxuryExpenses, id))
                    expense = GetLuxuryExpense(monthInfo.LuxuryExpenses, id);

                if (IsMonthlyExpense(monthInfo.MonthlyExpenses, id))
                    expense = GetMonthlyExpense(monthInfo.MonthlyExpenses, id);

                if (IsRequiredExpense(monthInfo.RequiredExpenses, id))
                    expense = GetRequiredExpense(monthInfo.RequiredExpenses, id);

                expenseList.Add(expense);
            }
            

            return expenseList;
        }

        public void DeleteExpense(int[] expenseIds)
        {
            foreach (var id in expenseIds)
            {
                _sqlDataAccessor.DeleteExpense(id);
            }
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

        private bool IsLuxuryExpense(List<Expense> luxuryExpeneses, int id)
        {
            foreach (var expense in luxuryExpeneses)
            {
                if (expense.Id == id)
                    return true;
            }
            return false;
        }

        private bool IsRequiredExpense(List<Expense> requiredExpeneses, int id)
        {
            foreach (var expense in requiredExpeneses)
            {
                if (expense.Id == id)
                    return true;
            }
            return false;
        }

        private bool IsMonthlyExpense(List<Expense> monthlyExpeneses, int id)
        {
            foreach (var expense in monthlyExpeneses)
            {
                if (expense.Id == id)
                    return true;
            }
            return false;
        }

        private Expense GetLuxuryExpense(List<Expense> luxuryExpeneses, int id)
        {
            return luxuryExpeneses.FirstOrDefault(x => x.Id == id);
        }

        private Expense GetRequiredExpense(List<Expense> requiredExpeneses, int id)
        {
            return requiredExpeneses.FirstOrDefault(x => x.Id == id);
        }

        private Expense GetMonthlyExpense(List<Expense> monthlyExpeneses, int id)
        {
            return monthlyExpeneses.FirstOrDefault(x => x.Id == id);
        }
    }
}