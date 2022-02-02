using PagedList;
using QLHTFastFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHTFastFood.Areas.Admin.Controllers
{
    public class FeedbackController : BaseController
    {
        // GET: Admin/Feedback
        QLFastFoodEntities db = new QLFastFoodEntities();
        public ActionResult Index(string id, int page = 1, int pageSize = 5)
        {
            if (id != null && id != "")
            {
                return View(db.FEEDBACKs.Where(x => x.Feedback_ID.StartsWith(id)).OrderBy(x => x.Feedback_ID).ToPagedList(page, pageSize));
            }
            else
            {
                return View(db.FEEDBACKs.ToList().OrderBy(n => n.Feedback_ID).ToPagedList(page, pageSize));
            }
        }


    }
}