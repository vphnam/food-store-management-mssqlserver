using PagedList;
using QLHTFastFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHTFastFood.Areas.Admin.Controllers
{
    public class DonViController : BaseController
    {
        QLFastFoodEntities db = new QLFastFoodEntities();
        // GET: Admin/DonVi
        public ActionResult Index(string id, int page = 1, int pageSize = 5)
        {
            if (id != null && id != "")
            {
                return View(db.DONVIs.Where(x => x.DonVi_ID.StartsWith(id)).OrderBy(x => x.DonVi_ID).ToPagedList(page, pageSize));
            }
            else
            {
                return View(db.DONVIs.ToList().OrderBy(n => n.DonVi_ID).ToPagedList(page, pageSize));
            }      
        }
        public ActionResult Create()
        {
            DONVI dv = new DONVI();
            return View(dv);
        }
        [HttpPost]
        public ActionResult Create(DONVI dvEntity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dvEntity.DonVi_ID = "1";
                    db.DONVIs.Add(dvEntity);
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
            return View(db.DONVIs.Find(id));
        }
        [HttpPost]
        public ActionResult Edit(DONVI dvEntity, string id)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    dvEntity.DonVi_ID = id;
                    db.Entry(dvEntity).State = System.Data.Entity.EntityState.Modified;
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
                return View(dvEntity);
            }
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(db.DONVIs.Where(s => s.DonVi_ID == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Delete(string id, DONVI dvEntity)
        {
            try
            {
                dvEntity = db.DONVIs.Where(s => s.DonVi_ID == id).FirstOrDefault();
                db.DONVIs.Remove(dvEntity);
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