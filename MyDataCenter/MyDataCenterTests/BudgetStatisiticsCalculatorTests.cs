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
        private List<double> _luxuryExpenseList;
        private List<double> _monthlyExpenseList;
        private List<double> _requiredExpenseList;
        private BudgetStatistics _budgetStatistics;
        private IBudgetStatisicsCalculator _budgetStatisicsCalculator;

        [SetUp]
        public void Setup()
        {
            _luxuryExpenseList = new List<double>();
            _monthlyExpenseList = new List<double>();
            _requiredExpenseList = new List<double>();
            _budgetStatistics = new BudgetStatistics();
            
            _budgetStatisicsCalculator = new BudgetStatisticsCalculator();
        }

        [TestCase(1,2,3)]
        public void CalculateRequiredSpentTotalTest(double requiredExpenseOne, double requiredExpenseTwo, double requiredExpenseThree)
        {
            var expected = requiredExpenseOne + requiredExpenseTwo + requiredExpenseThree;   
            _requiredExpenseList.Add(requiredExpenseOne);
            _requiredExpenseList.Add(requiredExpenseTwo);
            _requiredExpenseList.Add(requiredExpenseThree);

            _budgetStatisicsCalculator.CalculateBudgetStatistics(new List<double>(), _requiredExpenseList,
                new List<double>(), _budgetStatistics);

            Assert.AreEqual(expected, _budgetStatistics.RequiredTotalSpent);
        }

        [TestCase(100, 50)]
        [TestCase(100, 150)]
        [TestCase(100, 100)]
        public void CalculateMoneyLeftOverTest(double totalPay, double requiredSpent)
        {
            var expected = totalPay - requiredSpent;
            _budgetStatistics.TotalPay = totalPay;
            _requiredExpenseList.Add(requiredSpent);

            _budgetStatisicsCalculator.CalculateBudgetStatistics(new List<double>(), _requiredExpenseList,
                new List<double>(), _budgetStatistics);

            Assert.AreEqual(expected, _budgetStatistics.MoneyLeftoverToSpendOnLuxury);
        }

        [TestCase(1, 2, 3)]
        public void CalculateMonthlySpentTotalTest(double monthlyExpenseOne, double monthlyExpenseTwo, double monthlyExpenseThree)
        {
            var expected = monthlyExpenseOne + monthlyExpenseTwo + monthlyExpenseThree;
            _monthlyExpenseList.Add(monthlyExpenseOne);
            _monthlyExpenseList.Add(monthlyExpenseTwo);
            _monthlyExpenseList.Add(monthlyExpenseThree);

            _budgetStatisicsCalculator.CalculateBudgetStatistics(new List<double>(), new List<double>(),
                _monthlyExpenseList, _budgetStatistics);

            Assert.AreEqual(expected, _budgetStatistics.MonthlyTotalSpent);
        }

        [TestCase(1,2,3)]
        public void CalculateLuxurySpentTest(double luxuryExpenseOne, double luxuryExpenseTwo, double luxuryExpenseThree)
        {
            var expected = luxuryExpenseOne + luxuryExpenseTwo + luxuryExpenseThree;
            _luxuryExpenseList.Add(luxuryExpenseOne);
            _luxuryExpenseList.Add(luxuryExpenseTwo);
            _luxuryExpenseList.Add(luxuryExpenseThree);

            _budgetStatisicsCalculator.CalculateBudgetStatistics(_luxuryExpenseList, new List<double>(),
                new List<double>(), _budgetStatistics);

            Assert.AreEqual(expected, _budgetStatistics.LuxuryTotalSpent);
        }

        [TestCase(50, 25, 15)]
        [TestCase(50, 25, 25)]
        [TestCase(50, 25, 50)]
        public void CalculateTotalSpentPerMonthTest(double requiredSpent, double luxurySpent, double monthlySpent)
        {
            var expected = requiredSpent + luxurySpent + monthlySpent;
            _luxuryExpenseList.Add(luxurySpent);
            _monthlyExpenseList.Add(monthlySpent);
            _requiredExpenseList.Add(requiredSpent);
            _budgetStatistics.TotalPay = 0;

            _budgetStatisicsCalculator.CalculateBudgetStatistics(_luxuryExpenseList, _requiredExpenseList,
                _monthlyExpenseList, _budgetStatistics);
           
            Assert.AreEqual(expected, _budgetStatistics.TotalSpentPerMonth);
        }

        [TestCase(100, 50, 25, 15)]
        [TestCase(100, 50, 25, 25)]
        [TestCase(100, 50, 25, 50)]
        public void CalculateTotalRemainingTest(double totalPay, double requiredSpent, double luxurySpent, double monthlySpent)
        {
            var expected = totalPay - (requiredSpent + luxurySpent + monthlySpent);
            _luxuryExpenseList.Add(luxurySpent);
            _monthlyExpenseList.Add(monthlySpent);
            _requiredExpenseList.Add(requiredSpent);
            _budgetStatistics.TotalPay = totalPay;

            _budgetStatisicsCalculator.CalculateBudgetStatistics(_luxuryExpenseList, _requiredExpenseList,
                _monthlyExpenseList, _budgetStatistics);

            Assert.AreEqual(expected, _budgetStatistics.TotalRemaining);
        }
    }
}
