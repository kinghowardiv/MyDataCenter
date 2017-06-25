using NUnit.Framework;
using MyDataCenter.Business;
using MyDataCenter.Models.POCOS;
using Moq;
using System.Collections.Generic;

namespace MyDataCenterTests
{
    [TestFixture]
    public class MonthlyBudgetInfoAccessorTests
    {
        private IMonthlyBudgetInfoAccessor _monthlyBudgetInfoProvider;
        private Mock<ISqlDataAccessor> _sqlDataAccessorMock;
        private Mock<IMonthlyBudgetStatisicsCalculator> _budgetStatsCalculatorMock;
        private Month _currentMonthStub;
        private Expense _luxuryExpenseStub;
        private Expense _monthlyExpenseStub;
        private Expense _requiredExpenseStub;
        private List<Expense> _testExpenseList;

        [SetUp]
        public void Setup()
        {
            _luxuryExpenseStub = new Expense
            {
                Type = "Luxury",
                Id = 1
            };
            _monthlyExpenseStub = new Expense
            {
                Type = "Monthly",
                Id = 2
            };
            _requiredExpenseStub = new Expense
            {
                Type = "Required",
                Id = 3
            };

            _testExpenseList = new List<Expense>
            {
                _luxuryExpenseStub,
                _monthlyExpenseStub,
                _requiredExpenseStub
            };

            _currentMonthStub = new Month();

            _sqlDataAccessorMock = new Mock<ISqlDataAccessor>();
            _sqlDataAccessorMock.Setup(x => x.GetSingleMonthInfo(It.IsAny<int>(), It.IsAny<int>())).Returns(_currentMonthStub);
            _sqlDataAccessorMock.Setup(x => x.GetMonthlyExpenses(It.IsAny<int>(), It.IsAny<int>())).Returns(_testExpenseList);

            _budgetStatsCalculatorMock = new Mock<IMonthlyBudgetStatisicsCalculator>();
           
            _monthlyBudgetInfoProvider = new MonthlyBudgetInfoAccessor(_sqlDataAccessorMock.Object, _budgetStatsCalculatorMock.Object);
        }

        [Test]
        public void UpdateCurrentMonthlyInfoUpdatesMonthInfoCorrectlyTest()
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
        public void GetCurrentMonthInfoReturnsTheInfoForCurrentMonthTest()
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

        [Test]
        public void UpdateExpenseUpdatesExpenseCorrectlyTest()
        {
            _monthlyBudgetInfoProvider.SaveUpdatedExpenseInfo(1, 1, new Expense());

            _sqlDataAccessorMock.Verify(x => x.UpdateExpenseInfo(It.IsAny<Expense>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);   
        }

        [Test]
        public void GetExpensesToUpdateCanGetLuxuryExpenseTest()
        {
            PopulateMonthlyExpenseLists();
            var expenseIdList = new int[]{ 1 };

            var expectedExpenseList = _monthlyBudgetInfoProvider.GetExpensesToUpdate(_currentMonthStub, expenseIdList);

            Assert.AreEqual(_luxuryExpenseStub, expectedExpenseList[0]);
            Assert.AreEqual(expectedExpenseList[0].Type, "Luxury");
        }

        [Test]
        public void GetExpensesToUpdateCanGetMonthlyExpenseTest()
        {
            PopulateMonthlyExpenseLists();
            var expenseIdList = new int[] { 2 };

            var expectedExpenseList = _monthlyBudgetInfoProvider.GetExpensesToUpdate(_currentMonthStub, expenseIdList);

            Assert.AreEqual(_monthlyExpenseStub, expectedExpenseList[0]);
            Assert.AreEqual(expectedExpenseList[0].Type, "Monthly");
        }

        [Test]
        public void GetExpensesToUpdateCanGetRequiredExpenseTest()
        {
            PopulateMonthlyExpenseLists();
            var expenseIdList = new int[] { 3 };

            var expectedExpenseList = _monthlyBudgetInfoProvider.GetExpensesToUpdate(_currentMonthStub, expenseIdList);

            Assert.AreEqual(_requiredExpenseStub, expectedExpenseList[0]);
            Assert.AreEqual(expectedExpenseList[0].Type, "Required");
        }

        [Test]
        public void DeleteExpenseDeletesCorrectExpenseTest()
        {
            var expenseIds = new int[3];
            expenseIds[0] = 1;
            expenseIds[1] = 2;
            expenseIds[2] = 3;
            
            _monthlyBudgetInfoProvider.DeleteExpense(expenseIds);

            _sqlDataAccessorMock.Verify(x => x.DeleteExpense(It.IsAny<int>()), Times.Exactly(3));
        }

        [Test]
        public void CreateExpenseTest()
        {
            var testMonth = 5;
            var testYear = 2017;

            var testExpense = new Expense
            {
                Name = "test",
                Price = 5,
                Type = "Luxury",
                Id = 5
            };

            _monthlyBudgetInfoProvider.CreateExpense(testExpense, testMonth, testYear);

            _sqlDataAccessorMock.Verify(x => x.CreateExpense(testExpense, testMonth, testYear));
        }

        private void PopulateMonthlyExpenseLists()
        {
            _currentMonthStub.LuxuryExpenses = new List<Expense>
            {
                _luxuryExpenseStub
            };
            _currentMonthStub.MonthlyExpenses = new List<Expense>
            {
                _monthlyExpenseStub
            };
            _currentMonthStub.RequiredExpenses = new List<Expense>
            {
                _requiredExpenseStub
            };
        }
    }
}
