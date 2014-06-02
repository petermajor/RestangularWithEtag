﻿namespace Employees.Web.Controllers
{
    using System.Web.Mvc;

    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
