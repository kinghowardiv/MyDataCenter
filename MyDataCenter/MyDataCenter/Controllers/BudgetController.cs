﻿using System.Web.Mvc;
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
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var budgetInfoProvider = new MonthlyBudgetInfoAccessor(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            var currentMonthInfo = budgetInfoProvider.GetCurrentMonthInfo(month-1, year);

            ViewBag.Title = currentMonthInfo.Name + " " + year.ToString() + " Budget Raw Data"; 

            return View(currentMonthInfo);
        }

        public ActionResult UpdateCurrentMonthInfoView()
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var budgetInfoProvider = new MonthlyBudgetInfoAccessor(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            var monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month - 1, year);

            return View(monthInfo);
        }

        public ActionResult AllMonthsInfoView()
        {
            var budgetInfoProvider = new MonthlyBudgetInfoAccessor(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());
            var months = budgetInfoProvider.GetAllMonthsInfo();

            return View(months);
        }

        [HttpPost]
        public ActionResult SaveUpdatedMonthInfo(Month monthInfo)
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var budgetInfoProvider = new MonthlyBudgetInfoAccessor(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            budgetInfoProvider.UpdateCurrentMonthInfo(month - 1, year, monthInfo);
            ModelState.Clear();

            return View();
        }

        public ActionResult CreateNewMonth()
        {
            var month = new Month();

            return View(month);
        }

        [HttpPost]
        public ActionResult SaveNewMonthInfo(Month monthInfo)
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var budgetInfoProvider = new MonthlyBudgetInfoAccessor(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            budgetInfoProvider.CreateNewMonthInfo(monthInfo, month - 2, year);

            var months = budgetInfoProvider.GetAllMonthsInfo();

            return View("ViewAllMonthsInfo", months);
        }

        [HttpPost]
        public ActionResult UpdateMonthInfo(Month monthInfo, string saveButton, string deleteButton)
        {
            var view = UpdateOrDeleteMonthBasedOnButtonClick(monthInfo, saveButton, deleteButton);

            return view;
        }

        private ActionResult UpdateOrDeleteMonthBasedOnButtonClick(Month monthInfo, string saveButton, string deleteButton)
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var budgetInfoProvider = new MonthlyBudgetInfoAccessor(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            if (deleteButton != null)
            {
                budgetInfoProvider.DeleteMonth(monthInfo);

                monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month - 1, year);
                return View("DeleteExpense", monthInfo);
            }

            return View();
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
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var budgetInfoProvider = new MonthlyBudgetInfoAccessor(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());
            var monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month - 1, year);

            foreach (var expense in expenses)
            {
                budgetInfoProvider.SaveUpdatedExpenseInfo(month - 1, year, expense);
            }
            
            ModelState.Clear();

            return View(monthInfo);
        }

        [HttpPost]
        public ActionResult SaveNewExpense(Expense expense)
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var budgetInfoProvider = new MonthlyBudgetInfoAccessor(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            expense.Id = 81;
            budgetInfoProvider.CreateExpense(expense, month-1, year);            

            ModelState.Clear();

            var monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month - 1, year);

            return View("CurrentMonthInfo", monthInfo);
        }

        public ActionResult NewExpense()
        {
            var expense = new Expense();

            return View(expense);
        }

        private ActionResult UpdateOrDeleteExpenseBasedOnButtonClick(int[] expensesIds, string saveButton, string deleteButton)
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var budgetInfoProvider = new MonthlyBudgetInfoAccessor(new SqlDataAccessor(), new MonthlyBudgetStatisticsCalculator());

            var monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month - 1, year);
            var expenseList = budgetInfoProvider.GetExpensesToUpdate(monthInfo, expensesIds);

            if (deleteButton != null)
            {
                budgetInfoProvider.DeleteExpense(expensesIds);
                ViewBag.DeleteMessage = expensesIds.Count() + " Expenses Deleted";

                monthInfo = budgetInfoProvider.GetCurrentMonthInfo(month - 1, year);
                return View("DeleteExpense", monthInfo);
            }

            return View(expenseList);
        }

        #endregion
    }
}