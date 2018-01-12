using Newtonsoft.Json.Linq;
using PoloniexWeb.Helpers;
using PoloniexWeb.Interfaces;
using PoloniexWeb.Models;
using PoloniexWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PoloniexWeb.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            ProductEntitiesService = new ProductEntitiesService();
        }
        private IProductEntitiesService ProductEntitiesService { get; set; }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Warehouse()
        {
            return View();
        }
        public ActionResult ShoppingList()
        {
            return View();
        }
        //public ActionResult GetData(string url)
        //{
        //    return Json(ProductEntitiesService.GetData(url), JsonRequestBehavior.AllowGet);
        //}
    }
}