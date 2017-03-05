using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Collections.Generic;
using MyDataCenter.Business;
using MyDataCenter.Models.POCOS;
using Assert = NUnit.Framework.Assert;

namespace MyDataCenterTests
{
    [TestClass]
    public class BudgetStatisiticsCalculatorTests
    {
        private List<Expense> _luxuryExpenseList;
        private List<Expense> _monthlyExpenseList;
        private List<Expense> _requiredExpenseList;
        private IBudgetStatisicsCalculator _budgetStatisicsCalculator;
        private Month _currentMonthStub;

        [SetUp]
        public void Setup()
        {
            _luxuryExpenseList = new List<Expense>();
            _monthlyExpenseList = new List<Expense>();
            _requiredExpenseList = new List<Expense>();

            _currentMonthStub = new Month();
            _currentMonthStub.LuxuryExpenses = _luxuryExpenseList;
            _currentMonthStub.MonthlyExpenses = _monthlyExpenseList;
            _currentMonthStub.RequiredExpenses = _requiredExpenseList;

            _budgetStatisicsCalculator = new MonthlyBudgetStatisticsCalculator();
        }

        private Expense CreateTestExpense(double price)
        {
            return new Expense
            {
                Price = price
            };
        }

        [TestCase(1,2,3)]
        public void CalculateRequiredSpentTotalTest(double requiredExpenseOne, double requiredExpenseTwo, double requiredExpenseThree)
        {
            var expected = requiredExpenseOne + requiredExpenseTwo + requiredExpenseThree;   

            _requiredExpenseList.Add(CreateTestExpense(requiredExpenseOne));
            _requiredExpenseList.Add(CreateTestExpense(requiredExpenseTwo));
            _requiredExpenseList.Add(CreateTestExpense(requiredExpenseThree));

            _budgetStatisicsCalculator.CalculateBudgetStatistics(_currentMonthStub);

            Assert.AreEqual(expected, _currentMonthStub.BudgetStatistics.RequiredTotalSpent);
        }

        [TestCase(100, 50)]
        [TestCase(100, 150)]
        [TestCase(100, 100)]
        public void CalculateMoneyLeftOverTest(double totalPay, double requiredSpent)
        {
            var expected = totalPay - requiredSpent;
            _currentMonthStub.TotalPay = totalPay;
            _requiredExpenseList.Add(CreateTestExpense(requiredSpent));

            _budgetStatisicsCalculator.CalculateBudgetStatistics(_currentMonthStub);

            Assert.AreEqual(expected, _currentMonthStub.BudgetStatistics.MoneyLeftoverToSpendOnLuxury);
        }

        [TestCase(1, 2, 3)]
        public void CalculateMonthlySpentTotalTest(double monthlyExpenseOne, double monthlyExpenseTwo, double monthlyExpenseThree)
        {
            var expected = monthlyExpenseOne + monthlyExpenseTwo + monthlyExpenseThree;
            _monthlyExpenseList.Add(CreateTestExpense(monthlyExpenseOne));
            _monthlyExpenseList.Add(CreateTestExpense(monthlyExpenseTwo));
            _monthlyExpenseList.Add(CreateTestExpense(monthlyExpenseThree));

            _budgetStatisicsCalculator.CalculateBudgetStatistics(_currentMonthStub);

            Assert.AreEqual(expected, _currentMonthStub.BudgetStatistics.MonthlyTotalSpent);
        }

        [TestCase(1,2,3)]
        public void CalculateLuxurySpentTest(double luxuryExpenseOne, double luxuryExpenseTwo, double luxuryExpenseThree)
        {
            var expected = luxuryExpenseOne + luxuryExpenseTwo + luxuryExpenseThree;
            _luxuryExpenseList.Add(CreateTestExpense(luxuryExpenseOne));
            _luxuryExpenseList.Add(CreateTestExpense(luxuryExpenseTwo));
            _luxuryExpenseList.Add(CreateTestExpense(luxuryExpenseThree));

            _budgetStatisicsCalculator.CalculateBudgetStatistics(_currentMonthStub);

            Assert.AreEqual(expected, _currentMonthStub.BudgetStatistics.LuxuryTotalSpent);
        }

        [TestCase(50, 25, 15)]
        [TestCase(50, 25, 25)]
        [TestCase(50, 25, 50)]
        public void CalculateTotalSpentPerMonthTest(double requiredSpent, double luxurySpent, double monthlySpent)
        {
            var expected = requiredSpent + luxurySpent + monthlySpent;
            _luxuryExpenseList.Add(CreateTestExpense(luxurySpent));
            _monthlyExpenseList.Add(CreateTestExpense(monthlySpent));
            _requiredExpenseList.Add(CreateTestExpense(requiredSpent));
            _currentMonthStub.TotalPay = 0;

            _budgetStatisicsCalculator.CalculateBudgetStatistics(_currentMonthStub);
           
            Assert.AreEqual(expected, _currentMonthStub.BudgetStatistics.TotalSpentPerMonth);
        }

        [TestCase(100, 50, 25, 15)]
        [TestCase(100, 50, 25, 25)]
        [TestCase(100, 50, 25, 50)]
        public void CalculateTotalRemainingTest(double totalPay, double requiredSpent, double luxurySpent, double monthlySpent)
        {
            var expected = totalPay - (requiredSpent + luxurySpent + monthlySpent);
            _luxuryExpenseList.Add(CreateTestExpense(luxurySpent));
            _monthlyExpenseList.Add(CreateTestExpense(monthlySpent));
            _requiredExpenseList.Add(CreateTestExpense(requiredSpent));
            _currentMonthStub.TotalPay = totalPay;

            _budgetStatisicsCalculator.CalculateBudgetStatistics(_currentMonthStub);

            Assert.AreEqual(expected, _currentMonthStub.BudgetStatistics.TotalRemaining);
        }
    }
}
