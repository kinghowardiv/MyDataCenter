using System.Web.Mvc;
using MyDataCenter.Business;
using System;
using MyDataCenter.Models.POCOS;

namespace MyDataCenter.Controllers
{
    public class BudgetController : Controller
    {
        public ActionResult CurrentMonthInfo()
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var budgetInfoProvider = new MonthlyBudgetInfoProvider();

            var months = budgetInfoProvider.GetCurrentMonthInfo(month-1, year);

            return View(months);
        }

        public ActionResult UpdateCurrentMonthInfo()
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var budgetInfoProvider = new MonthlyBudgetInfoProvider();

            var months = budgetInfoProvider.GetCurrentMonthInfo(month - 1, year);

            return View(months);
        }

        [HttpPost]
        public ActionResult SaveUpdatedMonthInfo(Month monthInfo)
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var budgetInfoProvider = new MonthlyBudgetInfoProvider();

            budgetInfoProvider.UpdateCurrentMonthInfo(month - 1, year, monthInfo);
            ModelState.Clear();

            return View();
        }

    }
}