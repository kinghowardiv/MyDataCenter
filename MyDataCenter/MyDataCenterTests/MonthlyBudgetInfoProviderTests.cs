using NUnit.Framework;
using MyDataCenter.Business;
using MyDataCenter.Models.POCOS;
using Moq;
using System.Collections.Generic;

namespace MyDataCenterTests
{
    [TestFixture]
    public class MonthlyBudgetInfoProviderTests
    {
        private IMonthlyBudgetInfoProvider _monthlyBudgetInfoProvider;
        private Mock<ISqlDataAccessor> _sqlDataAccessorMock;
        private Mock<IBudgetStatisicsCalculator> _budgetStatsCalculatorMock;
        private Month _currentMonthStub;
        private Expense _luxuryExpenseStub;
        private Expense _monthlyExpenseStub;
        private Expense _requiredExpenseStub;

        [SetUp]
        public void Setup()
        {
            _luxuryExpenseStub = new Expense
            {
                Type = "Luxury"
            };
            _monthlyExpenseStub = new Expense
            {
                Type = "Monthly"
            };
            _requiredExpenseStub = new Expense
            {
                Type = "Required"
            };

            var testExpenseList = new List<Expense>
            {
                _luxuryExpenseStub,
                _monthlyExpenseStub,
                _requiredExpenseStub
            };

            _currentMonthStub = new Month();

            _sqlDataAccessorMock = new Mock<ISqlDataAccessor>();
            _sqlDataAccessorMock.Setup(x => x.GetSingleMonthInfo(It.IsAny<int>(), It.IsAny<int>())).Returns(_currentMonthStub);
            _sqlDataAccessorMock.Setup(x => x.GetMonthlyExpenses(It.IsAny<int>(), It.IsAny<int>())).Returns(testExpenseList);

            _budgetStatsCalculatorMock = new Mock<IBudgetStatisicsCalculator>();
           
            _monthlyBudgetInfoProvider = new MonthlyBudgetInfoProvider(_sqlDataAccessorMock.Object, _budgetStatsCalculatorMock.Object);
        }

        [Test]
        public void UpdateCurrentMonthlyInfoCallsUpdateInSqlAccessorTest()
        {
            _monthlyBudgetInfoProvider.UpdateCurrentMonthInfo(1, 1, new Month());

            _sqlDataAccessorMock.Verify(x => x.UpdateMonthlyInfo(It.IsAny<Month>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void GetCurrentMonthInfoTest()
        {
            var monthFromProvider = _monthlyBudgetInfoProvider.GetCurrentMonthInfo(1, 1);

            Assert.AreEqual(_currentMonthStub, monthFromProvider);
        }

        [Test]
        public void GetCurrentMonthInfoCallsGetSingleMonthInfoInSqlAccessorTest()
        {
            var month = _monthlyBudgetInfoProvider.GetCurrentMonthInfo(1, 1);

            _sqlDataAccessorMock.Verify(x => x.GetSingleMonthInfo(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void RequiredExpensesWereAddedCorrectlyTest()
        {
            var month = _monthlyBudgetInfoProvider.GetCurrentMonthInfo(1, 1);

            Assert.AreEqual(_requiredExpenseStub, month.RequiredExpenses[0]);
        }

        [Test]
        public void MonthlyExpensesWereAddedCorrectlyTest()
        {
            var month = _monthlyBudgetInfoProvider.GetCurrentMonthInfo(1, 1);

            Assert.AreEqual(_monthlyExpenseStub, month.MonthlyExpenses[0]);
        }

        [Test]
        public void LuxuryExpensesWereAddedCorrectlyTest()
        {
            var month = _monthlyBudgetInfoProvider.GetCurrentMonthInfo(1, 1);

            Assert.AreEqual(_luxuryExpenseStub, month.LuxuryExpenses[0]);
        }
    }
}
