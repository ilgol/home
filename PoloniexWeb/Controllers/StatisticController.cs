using PoloniexWeb.Interfaces;
using PoloniexWeb.Models;
using PoloniexWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PoloniexWeb.Controllers
{
    [Authorize]
    public class StatisticController : Controller
    {
        public StatisticController()
        {
            StatisticService = new StatisticService();
        }
        public IStatisticService StatisticService { get; set; }
        // GET: Statistic
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
        }

        public ActionResult GetData(string startDate, string endDate)
        {
            return Json(StatisticService.GetData(startDate, endDate), JsonRequestBehavior.AllowGet);
        }
    }
}