using Diploma.Interfaces;
using Diploma.Models;
using Diploma.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Diploma.Controllers
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
            return View(ProductEntitiesService.GetProducts());
        }
        [HttpPost]
        public ActionResult UpdateEntities(ProductModel[] toUpdate, ProductModel[] toDelete)
        {
            ProductEntitiesService.UpdateEntities(toUpdate, toDelete);
            return Content("OK");
        }
        public ActionResult ShoppingList()
        {
            return View(ProductEntitiesService.GetShoppingList());
        }
        //public ActionResult GetData(string url)
        //{
        //    return Json(ProductEntitiesService.GetData(url), JsonRequestBehavior.AllowGet);
        //}
    }
}