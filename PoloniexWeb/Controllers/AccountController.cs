using Data;
using PoloniexWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PoloniexWeb.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                using (var db = new homeEntities())
                {
                    var user = db.Users.FirstOrDefault(u => u.UserName == model.Name && u.Password == model.Password);
                    if (user != null)
                    {
                        string decodedUrl = "";
                        if (!string.IsNullOrEmpty(ReturnUrl))
                            decodedUrl = Server.UrlDecode(ReturnUrl);

                        FormsAuthentication.SetAuthCookie(model.Name, true);

                        if (Url.IsLocalUrl(decodedUrl))
                        {
                            return Redirect(decodedUrl);
                        }
                        else
                        {
                            return RedirectToAction("Poloniex", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                    }
                }
            }
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}