using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyDataCenter.Business;


namespace MyDataCenter.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var emailProvier = new EmailProvider();
            emailProvier.GetEmails();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Bob";
            var sqlConnector = new SqlDataAccessor();
            var months = sqlConnector.GetSingleMonthInfo(2, 2017);

            return View(months);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
       //     var sqlConnector = new SqlDataAccessor();
        //    sqlConnector.Update();
            return View();
        }
    }
}