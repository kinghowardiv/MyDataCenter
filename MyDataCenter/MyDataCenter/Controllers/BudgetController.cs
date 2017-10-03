using System.Web.Mvc;
using MyDataCenter.Business;
using System;
using MyDataCenter.Models.POCOS;
using System.Linq;
using System.Collections.Generic;

namespace MyDataCenter.Controllers
{
    public class BudgetController : Controller
    {

        #region Month
        public ActionResult CurrentMonthInfoView()
        {
            //  var month = DateTime.Now.Month;
            //  var year = DateTime.Now.Year;

            var month = 5;
            var year = 2017;

            var budgetInfoProvider = new MonthlyBudgetInfoAccessor(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            //var currentMonthInfo = budgetInfoProvider.GetCurrentMonthInfo(month-2, year);

            var currentMonthInfo = budgetInfoProvider.GetCurrentMonthInfo(month, year);

            ViewBag.Title = currentMonthInfo.Name + " " + year.ToString() + " Budget Raw Data"; 

            return View(currentMonthInfo);
        }

        public ActionResult UpdateCurrentMonthInfoView()
        {
            // var month = DateTime.Now.Month;
            //  var year = DateTime.Now.Year;

            var month = 5;
            var year = 2017;

            var budgetInfoProvider = new MonthlyBudgetInfoAccessor(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            // var monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month - 1, year);

            var monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month, year);

            return View(monthInfo);
        }

        public ActionResult AllMonthsInfoView()
        {
            var budgetInfoProvider = new MonthlyBudgetInfoAccessor(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());
            var months = budgetInfoProvider.GetAllMonthsInfo();

            return View(months);
        }

        public ActionResult DisplayMonthInfoForEdditing()
        {
            var month = 5;
            var year = 2017;

            var budgetInfoProvider = new MonthlyBudgetInfoAccessor(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            // budgetInfoProvider.UpdateCurrentMonthInfo(month - 1, year, monthInfo);
            var monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month, year);

            ModelState.Clear();

            return View("SaveUpdatedMonthInfo", monthInfo);
        }

        //this method is both the get and post. need to separate
        [HttpPost]
        public ActionResult SaveUpdatedMonthInfo(Month monthInfo)
        {
            //    var month = DateTime.Now.Month;
            //   var year = DateTime.Now.Year;

            var month = 5;
            var year = 2017;

            var budgetInfoProvider = new MonthlyBudgetInfoAccessor(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            // budgetInfoProvider.UpdateCurrentMonthInfo(month - 1, year, monthInfo);

            budgetInfoProvider.UpdateCurrentMonthInfo(month, year, monthInfo);
            var updatedMonthInfo = budgetInfoProvider.GetCurrentMonthInfo(month, year);

            ModelState.Clear();

            return View("CurrentMonthInfoView", updatedMonthInfo);
        }

        public ActionResult CreateNewMonth()
        {
            var month = new Month();

            return View(month);
        }

        [HttpPost]
        public ActionResult SaveNewMonthInfo(Month monthInfo)
        {
            //   var month = DateTime.Now.Month;
            //  var year = DateTime.Now.Year;

            var month = 5;
            var year = 2017;

            var budgetInfoProvider = new MonthlyBudgetInfoAccessor(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            // budgetInfoProvider.CreateNewMonthInfo(monthInfo, month - 2, year);


            budgetInfoProvider.CreateNewMonthInfo(monthInfo, month, year);

            var months = budgetInfoProvider.GetAllMonthsInfo();

            return View("ViewAllMonthsInfo", months);
        }

        [HttpPost]
        public ActionResult DeleteMonthInfo(string monthId)
        {
            var budgetInfoProvider = new MonthlyBudgetInfoAccessor(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            budgetInfoProvider.DeleteMonth(monthId);

            return View("AllMonthsInfoView");
        }

        #endregion

        #region Expense

        [HttpPost]
        public ActionResult UpdateExpenseInfo(int[] expensesIds, string saveButton, string deleteButton)
        {
            var view = UpdateOrDeleteExpenseBasedOnButtonClick(expensesIds, saveButton, deleteButton);

            return view;
        }


        [HttpPost]
        public ActionResult SaveUpdatedExpenseInfo(ICollection<Expense> expenses)
        {
            //  var month = DateTime.Now.Month;
            //  var year = DateTime.Now.Year;

            var month = 5;
            var year = 2017;

            var budgetInfoProvider = new MonthlyBudgetInfoAccessor(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            //var monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month - 1, year);


            var monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month, year);

            foreach (var expense in expenses)
            {
                //budgetInfoProvider.SaveUpdatedExpenseInfo(month - 1, year, expense);\

                budgetInfoProvider.SaveUpdatedExpenseInfo(month, year, expense);
            }
            
            ModelState.Clear();

            return View(monthInfo);
        }

        [HttpPost]
        public ActionResult SaveNewExpense(Expense expense)
        {
            //  var month = DateTime.Now.Month;
            //  var year = DateTime.Now.Year;

            var month = 5;
            var year = 2017;

            var budgetInfoProvider = new MonthlyBudgetInfoAccessor(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            expense.Id = 81;
            //budgetInfoProvider.CreateExpense(expense, month-1, year);            

            budgetInfoProvider.CreateExpense(expense, month, year);

            ModelState.Clear();

            //var monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month - 1, year);

            var monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month, year);

            return View("CurrentMonthInfo", monthInfo);
        }

        public ActionResult NewExpense()
        {
            var expense = new Expense();

            return View(expense);
        }

        private ActionResult UpdateOrDeleteExpenseBasedOnButtonClick(int[] expensesIds, string saveButton, string deleteButton)
        {
            //   var month = DateTime.Now.Month;
            //   var year = DateTime.Now.Year;

            var month = 5;
            var year = 2017;

            var budgetInfoProvider = new MonthlyBudgetInfoAccessor(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            // var monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month - 2, year);

            var monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month, year);

            var expenseList = budgetInfoProvider.GetExpensesToUpdate(monthInfo, expensesIds);

            if (deleteButton != null)
            {
                budgetInfoProvider.DeleteExpense(expensesIds);
                ViewBag.DeleteMessage = expensesIds.Count() + " Expenses Deleted";

              //  monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month - 1, year);

                monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month, year);
                return View("DeleteExpense", monthInfo);
            }

            return View(expenseList);
        }

        #endregion
    }
}