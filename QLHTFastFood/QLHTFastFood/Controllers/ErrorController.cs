using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHTFastFood.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            ViewBag.StatusCode = Response.StatusCode;
            ViewBag.Dis = Response.StatusDescription;
            
            return View("NotFound");
        }
    }
}