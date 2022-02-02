using PagedList;
using QLHTFastFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHTFastFood.Areas.Admin.Controllers
{
    public class KhuyenMaiController : BaseController
    {
        // GET: Admin/KhuyenMai
        QLFastFoodEntities db = new QLFastFoodEntities();
        public ActionResult Index(string id, int page = 1, int pageSize = 5)
        {
            if (id != null && id != "")
            {
                return View(db.KHUYENMAIs.Where(x => x.KhuyenMai_ID.StartsWith(id)).OrderBy(x => x.KhuyenMai_ID).ToPagedList(page, pageSize));
            }
            else
            {
                return View(db.KHUYENMAIs.ToList().OrderBy(n => n.KhuyenMai_ID).ToPagedList(page, pageSize));
            }     
        }
        public ActionResult Create()
        {
            KHUYENMAI km = new KHUYENMAI();
            return View(km);
        }
        [HttpPost]
        public ActionResult Create(KHUYENMAI kmEntity)
        {
            double x = (double)kmEntity.PhanTramKhuyenMai / 100;
            kmEntity.PhanTramKhuyenMai = x;
            if (ModelState.IsValid)
            {
                try
                {
                    if(kmEntity.NgayBatDau >= kmEntity.NgayKetThuc)
                    {
                        ModelState.AddModelError("", "Ngày bắt đầu không được sau ngày kết thúc!!");

                    }
                    if (kmEntity.PhanTramKhuyenMai > 1 || kmEntity.PhanTramKhuyenMai <= 0)
                    {
                        ModelState.AddModelError("", "Phần trăm khuyến mãi chỉ trong phạm vi 0 - 100");

                    }
                    kmEntity.KhuyenMai_ID = "1";
                    db.KHUYENMAIs.Add(kmEntity);
                    db.SaveChanges();
                    SetAlert("Tạo mới khuyến mãi thành công", "success");
                    return RedirectToAction("Index");
                }
                catch
                {
                    SetAlert("Tạo mới khuyến mãi thất bại", "error");
                    return View(kmEntity);
                }
            }
            else
            {
                return View();
            }

        }
        public ActionResult Edit(string id)
        {
            KHUYENMAI km = db.KHUYENMAIs.Find(id);
            km.PhanTramKhuyenMai = km.PhanTramKhuyenMai * 100;
            return View(km);
        }
        [HttpPost]
        public ActionResult Edit(KHUYENMAI kmEntity, string id)
        {
            try
            {
                double x = (double)kmEntity.PhanTramKhuyenMai / 100;
                kmEntity.PhanTramKhuyenMai = x;
                if (ModelState.IsValid)
                {
                    if(kmEntity.NgayBatDau >= kmEntity.NgayKetThuc)
                        {
                        ModelState.AddModelError("", "Ngày bắt đầu không được sau ngày kết thúc!!");

                    }
                    if (kmEntity.PhanTramKhuyenMai > 1 || kmEntity.PhanTramKhuyenMai <= 0)
                    {
                        ModelState.AddModelError("", "Phần trăm khuyến mãi chỉ trong phạm vi 0 - 100");

                    }
                    kmEntity.KhuyenMai_ID = id;
                    db.Entry(kmEntity).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    SetAlert("Cập nhật thông tin khuyến mãi thành công", "success");
                    return RedirectToAction("Index");   
                }
            }
            catch
            {
                SetAlert("Không được để trống thông tin, cập nhật thất bại!", "error");
                return View(kmEntity);
            }
            return View(kmEntity);

        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(db.KHUYENMAIs.Where(s => s.KhuyenMai_ID == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Delete(string id, KHUYENMAI kmEntity)
        {
            try
            {
                kmEntity = db.KHUYENMAIs.Where(s => s.KhuyenMai_ID == id).FirstOrDefault();
                db.KHUYENMAIs.Remove(kmEntity);
                db.SaveChanges();
                SetAlert("Xóa thông tin khuyến mãi thành công", "success");
                return RedirectToAction("Index");
            }
            catch
            {
                SetAlert("Xóa thông tin khuyến mãi thất bại", "error");
                return RedirectToAction("Index");
            }
        }
    }
}