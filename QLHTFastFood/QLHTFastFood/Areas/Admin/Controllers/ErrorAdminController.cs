using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHTFastFood.Areas.Admin.Controllers
{
    public class ErrorAdminController : Controller
    {
        // GET: Admin/ErrorAdmin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            ViewBag.StatusCodeAd = Response.StatusCode;
            ViewBag.DisAD = Response.StatusDescription;
            return View("NotFound");
        }
    }
}