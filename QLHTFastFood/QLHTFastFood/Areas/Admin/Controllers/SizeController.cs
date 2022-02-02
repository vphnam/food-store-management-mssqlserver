using PagedList;
using QLHTFastFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHTFastFood.Areas.Admin.Controllers
{
    public class SizeController : BaseController
    {
        QLFastFoodEntities db = new QLFastFoodEntities();
        // GET: Admin/Size
        public ActionResult Index(string id, int page = 1, int pageSize = 5)
        {
            if (id != null && id != "")
            {
                return View(db.SIZEs.Where(x => x.Size_ID.StartsWith(id)).OrderBy(x => x.Size_ID).ToPagedList(page, pageSize));
            }
            else
            {
                return View(db.SIZEs.ToList().OrderBy(n => n.Size_ID).ToPagedList(page, pageSize));
            }  
        }
        public ActionResult Create()
        {
            SIZE dv = new SIZE();
            return View(dv);
        }
        [HttpPost]
        public ActionResult Create(SIZE sEntity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    sEntity.Size_ID = "1";
                    db.SIZEs.Add(sEntity);
                    db.SaveChanges();
                    SetAlert("Tạo mới đơn vị thành công", "success");
                    return RedirectToAction("Index");
                }
                catch
                {
                    SetAlert("Tạo mới đơn vị thất bại", "error");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View();
            }

        }
        public ActionResult Edit(string id)
        {
            return View(db.SIZEs.Find(id));
        }
        [HttpPost]
        public ActionResult Edit(SIZE sEntity, string id)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    sEntity.Size_ID = id;
                    db.Entry(sEntity).State = System.Data.Entity.EntityState.Modified;
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
                return View(sEntity);
            }
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(db.SIZEs.Where(s => s.Size_ID == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Delete(string id, SIZE sEntity)
        {
            try
            {
                sEntity = db.SIZEs.Where(s => s.Size_ID == id).FirstOrDefault();
                db.SIZEs.Remove(sEntity);
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