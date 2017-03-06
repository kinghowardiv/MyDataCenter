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

        public ActionResult CurrentMonthInfo()
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var budgetInfoProvider = new MonthlyBudgetInfoProvider(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            var currentMonthInfo = budgetInfoProvider.GetCurrentMonthInfo(month-1, year);

            ViewBag.Title = currentMonthInfo.Name + " " + year.ToString() + " Budget Raw Data"; 

            return View(currentMonthInfo);
        }

        public ActionResult UpdateCurrentMonthInfo()
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var budgetInfoProvider = new MonthlyBudgetInfoProvider(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            var monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month - 1, year);

            return View(monthInfo);
        }

        [HttpPost]
        public ActionResult SaveUpdatedMonthInfo(Month monthInfo)
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var budgetInfoProvider = new MonthlyBudgetInfoProvider(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            budgetInfoProvider.UpdateCurrentMonthInfo(month - 1, year, monthInfo);
            ModelState.Clear();

            return View();
        }

        [HttpPost]
        public ActionResult SelectUpdatedExpenseInfoToSave(Month monthInfo, int[] expensesIds)
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var budgetInfoProvider = new MonthlyBudgetInfoProvider(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month - 1, year);
            var expenseList = budgetInfoProvider.GetExpensesToUpdate(monthInfo, expensesIds);       

            return View(expenseList);
        }

        [HttpPost]

        public ActionResult SaveUpdatedExpenseInfo(ICollection<Expense> expenses)
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var budgetInfoProvider = new MonthlyBudgetInfoProvider(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());
            var monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month - 1, year);

            foreach (var expense in expenses)
            {
                budgetInfoProvider.UpdateExpenseInfo(month - 1, year, expense);
            }
            
            ModelState.Clear();

            return View(monthInfo);
        }

        [HttpPost]
        public ActionResult DeleteExpense(Month monthInfo, int[] expensesIds)
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var budgetInfoProvider = new MonthlyBudgetInfoProvider(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            budgetInfoProvider.DeleteExpense(expensesIds);

            ViewBag.DeleteMessage = expensesIds.Count() + " Expenses Deleted";

            return View();
        }
    }
}