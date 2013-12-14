using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WooAuth.Controllers
{
    public class TokenController : Controller
    {
        //
        // GET: /Token/
        public ActionResult Index(string token)
        {
            ViewBag.Token = token;
            return View();
        }
	}
}