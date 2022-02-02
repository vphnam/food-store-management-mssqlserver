using QLHTFastFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHTFastFood.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        QLFastFoodEntities db = new QLFastFoodEntities();
        // GET: Admin/Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Loai()
        {
            var loai = db.LOAIs.OrderBy(n => n.Loai_ID).ToList();
            return PartialView(loai);
        }
    }
}