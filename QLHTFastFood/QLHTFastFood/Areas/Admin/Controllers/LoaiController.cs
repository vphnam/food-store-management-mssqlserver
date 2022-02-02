using PagedList;
using QLHTFastFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHTFastFood.Areas.Admin.Controllers
{
    public class LoaiController : BaseController
    {
        // GET: Admin/Loai
        QLFastFoodEntities db = new QLFastFoodEntities();
        public ActionResult Index(string id, int page = 1, int pageSize = 5)
        {
            if (id != null && id != "")
            {
                return View(db.LOAIs.Where(x => x.Loai_ID.StartsWith(id)).OrderBy(x => x.Loai_ID).ToPagedList(page, pageSize));
            }
            else
            {
                return View(db.LOAIs.ToList().OrderBy(n => n.Loai_ID).ToPagedList(page, pageSize));
            }     
        }
        public ActionResult Create()
        {
            LOAI loai = new LOAI();
            return View(loai);
        }
        [HttpPost]
        public ActionResult Create(LOAI loaiEntity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    loaiEntity.Loai_ID = "1";
                    db.LOAIs.Add(loaiEntity);
                    db.SaveChanges();
                    SetAlert("Tạo mới đơn vị thành công", "success");
                    return RedirectToAction("Index");
                }
                catch
                {
                    SetAlert("Tạo mới đơn vị thất bại", "error");
                    return View(loaiEntity);
                }
            }
            else
            {
                return View();
            }

        }
        public ActionResult Edit(string id)
        {
            return View(db.LOAIs.Find(id));
        }
        [HttpPost]
        public ActionResult Edit(LOAI loaiEntity, string id)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    loaiEntity.Loai_ID = id;
                    db.Entry(loaiEntity).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    SetAlert("Cập nhật thông tin đơn vị thành công", "success");
                    return RedirectToAction("Index");
                }
                catch
                {
                    SetAlert("Cập nhật thông tin đơn vị thất bại", "error");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(loaiEntity);
            }
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(db.LOAIs.Where(s => s.Loai_ID == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Delete(string id, LOAI loaiEntity)
        {
            try
            {
                loaiEntity = db.LOAIs.Where(s => s.Loai_ID == id).FirstOrDefault();
                db.LOAIs.Remove(loaiEntity);
                db.SaveChanges();
                SetAlert("Xóa thông tin đơn vị thành công", "success");
                return RedirectToAction("Index");
            }
            catch
            {
                SetAlert("Xóa thông tin đơn vị thất bại", "error");
                return RedirectToAction("Index");
            }
        }
    }
}