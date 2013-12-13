using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using core.Business;

namespace web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISearchReaderService _reader;

        public HomeController(ISearchReaderService reader)
        {
            _reader = reader;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
