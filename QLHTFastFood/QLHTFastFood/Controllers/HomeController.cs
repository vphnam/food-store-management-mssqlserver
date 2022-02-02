using QLHTFastFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHTFastFood.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        QLFastFoodEntities db = new QLFastFoodEntities();
        public List<DisplayMon> LayDSMon()
        {
            var mon = db.MONs.OrderBy(n => n.Mon_ID).ToList();
            List<DisplayMon> lstDisMon = new List<DisplayMon>();
            foreach (var item in mon)
            {
                DisplayMon disMon = new DisplayMon();
                disMon.Mon_ID = item.Mon_ID;
                disMon.TenMon = item.TenMon;
                disMon.Loai_ID = item.Loai_ID;
                disMon.Size_ID = item.Size_ID;
                disMon.DonVi_ID = item.DonVi_ID;
                disMon.AnhMon = item.AnhMon;
                disMon.DonGia = item.DonGia;
                disMon.KhuyenMai_ID = item.KhuyenMai_ID;
                if (disMon.KhuyenMai_ID != null)
                {
                    var km = db.KHUYENMAIs.Find(disMon.KhuyenMai_ID);
                    double phantramkm = (double)km.PhanTramKhuyenMai;
                    disMon.GiaKM = (double)item.DonGia - ((double)item.DonGia * phantramkm);
                }
                else
                    disMon.GiaKM = 0;
                disMon.MoTa = item.MoTa;
                lstDisMon.Add(disMon);
            }
            return lstDisMon;
        }
        public List<NGUYENLIEU> LayDSNlAo()
        {
            List<NGUYENLIEU> lstNL = Session["NguyenLieu"] as List<NGUYENLIEU>;
            if(lstNL == null)
            {
                lstNL = db.NGUYENLIEUx.ToList();
                Session["NguyenLieu"] = lstNL;
            }
            return lstNL;

        }
        public ActionResult Index(string priceFrom, string priceTo)
        {
            List<NGUYENLIEU> lstNL = LayDSNlAo();
            if(priceFrom != null && priceTo != null && priceFrom != "" && priceTo != "")
            {
                List<DisplayMon> lstDisMon = LayDSMon();
                return View(lstDisMon.Where(x => x.DonGia >= decimal.Parse(priceFrom) && x.DonGia <= decimal.Parse(priceTo) ||
                x.GiaKM >= double.Parse(priceFrom) && x.GiaKM <= double.Parse(priceTo)).ToList());
            }
            else
            {
                List<DisplayMon> lstDisMon = LayDSMon();
                return View(lstDisMon);
            }
        }
        public ActionResult Detail(string id)
        {
            if (id != null)
            {
                List<DisplayMon> lstDisMon = LayDSMon();
                DisplayMon disMon = lstDisMon.Where(x => x.Mon_ID == id).FirstOrDefault();
                return View(disMon);
            }
             else
            {
                return RedirectToAction("Index");
            }
          
        }
        public ActionResult MonTheoLoai(string id)
        {
            List<NGUYENLIEU> lstNL = LayDSNlAo();
            LOAI loai = db.LOAIs.Find(id);
            List<DisplayMon> lstDisMon = LayDSMon();
            ViewBag.TenLoai = loai.TenLoai;
            return View(lstDisMon.Where(n => n.Loai_ID == id).OrderBy(n => n.Mon_ID).ToList());
        }
        public ActionResult Loai()
        {
            return PartialView(db.LOAIs.OrderBy(n => n.Loai_ID).ToList());
        }
        public JsonResult GetSearchValue(string search)
        {
            List<MonSearchingModel> allsearch = db.MONs.Where(x => x.TenMon.Contains(search)).Select(x => new MonSearchingModel
            {
                Mon_ID = x.Mon_ID,
                TenMon = x.TenMon
            }).ToList();
            return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}