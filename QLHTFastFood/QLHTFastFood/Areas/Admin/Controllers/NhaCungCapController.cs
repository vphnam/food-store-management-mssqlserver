using PagedList;
using QLHTFastFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHTFastFood.Areas.Admin.Controllers
{
    public class NhaCungCapController : BaseController
    {
        // GET: Admin/NhaCungCap
        QLFastFoodEntities db = new QLFastFoodEntities();
        // GET: Admin/Supplier
        public ActionResult Index(string id, int page = 1, int pageSize = 5)
        {
            if (id != null && id != "")
            {
                return View(db.NHACUNGCAPs.Where(x => x.NCC_ID.StartsWith(id)).OrderBy(x => x.NCC_ID).ToPagedList(page, pageSize));
            }
            else
            {
                return View(db.NHACUNGCAPs.ToList().OrderBy(n => n.NCC_ID).ToPagedList(page, pageSize));
            }  
        }
        public ActionResult Create()
        {
            NHACUNGCAP ncc = new NHACUNGCAP();
            return View(ncc);
        }
        [HttpPost]
        public ActionResult Create(NHACUNGCAP nccEntity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    nccEntity.NCC_ID = "1";
                    db.NHACUNGCAPs.Add(nccEntity);
                    db.SaveChanges();
                    SetAlert("Tạo mới nhà cung cấp thành công", "success");
                    return RedirectToAction("Index");
                }
                catch
                {
                    SetAlert("Tạo mới nhà cung cấp thất bại", "error");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(nccEntity);
            }

        }
        public ActionResult Edit(string id)
        {
            return View(db.NHACUNGCAPs.Find(id));
        }
        [HttpPost]
        public ActionResult Edit(NHACUNGCAP nccEntity, string id)
        {

            if (ModelState.IsValid)
            {   
                try
                {
                    nccEntity.NCC_ID = id;
                    db.Entry(nccEntity).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    SetAlert("Cập nhật thông tin nhà cung cấp thành công", "success");
                    return RedirectToAction("Index");
                }          
                catch
                {
                    SetAlert("Cập nhật thông tin nhà cung cấp thất bại", "error");
                    return RedirectToAction("Index");
                }
            }
            return View(nccEntity);
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(db.NHACUNGCAPs.Where(s => s.NCC_ID == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Delete(string id, NHACUNGCAP nccEntity)
        {
            try
            {
                nccEntity = db.NHACUNGCAPs.Where(s => s.NCC_ID == id).FirstOrDefault();
                db.NHACUNGCAPs.Remove(nccEntity);
                db.SaveChanges();
                SetAlert("Xóa thông tin nhà cung cấp thành công", "success");
                return RedirectToAction("Index");
            }
            catch
            {
                SetAlert("Xóa thông tin nhà cung cấp thất bại", "error");
                return RedirectToAction("Index");
            }
        }
    }
}