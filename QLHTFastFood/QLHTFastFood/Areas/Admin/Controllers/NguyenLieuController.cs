using PagedList;
using QLHTFastFood.Areas.Admin.Models;
using QLHTFastFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHTFastFood.Areas.Admin.Controllers
{
    public class NguyenLieuController : BaseController
    {
        // GET: Admin/NguyenLieu
        QLFastFoodEntities db = new QLFastFoodEntities();
        public ActionResult Index(string id, int page = 1, int pageSize = 5)
        {
            if (id != null && id != "")
            {
                return View(db.NGUYENLIEUx.Where(x => x.NguyenLieu_ID.StartsWith(id)).OrderBy(x => x.NguyenLieu_ID).ToPagedList(page, pageSize));
            }
            else
            {
                return View(db.NGUYENLIEUx.ToList().OrderBy(n => n.NguyenLieu_ID).ToPagedList(page, pageSize));
            }          
        }
        public ActionResult Create()
        {
            NGUYENLIEU ncc = new NGUYENLIEU();
            return View(ncc);
        }
        [HttpPost]
        public ActionResult Create(NGUYENLIEU nlEntity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    nlEntity.NguyenLieu_ID = "1";
                    db.NGUYENLIEUx.Add(nlEntity);
                    db.SaveChanges();
                    SetAlert("Tạo mới nguyên liệu thành công", "success");
                    return RedirectToAction("Index");
                }
                catch
                {
                    SetAlert("Tạo mới nguyên liệu thất bại", "error");
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
            return View(db.NGUYENLIEUx.Find(id));
        }
        [HttpPost]
        public ActionResult Edit(NGUYENLIEU nlEntity, string id)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    nlEntity.NguyenLieu_ID = id;
                    db.Entry(nlEntity).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    SetAlert("Cập nhật thông tin nguyên liệu thành công", "success");
                    return RedirectToAction("Index");
                }
                catch
                {
                    SetAlert("Cập nhật thông tin nguyên liệu thất bại", "error");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(nlEntity);
            }
        }
        public ActionResult EditSL(string id)
        {
            NGUYENLIEU nl = new NGUYENLIEU();
            nl = db.NGUYENLIEUx.Find(id);
            NguyenLieuModel nlModel = new NguyenLieuModel();
            nlModel.MaNguyenLieu = nl.NguyenLieu_ID;
            nlModel.SoLuongTon = (double)nl.SoLuongTon;
            return View(nlModel);
        }
        [HttpPost]
        public ActionResult EditSL(NguyenLieuModel nl, string id)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    nl.MaNguyenLieu = id;
                    NGUYENLIEU nlEntity = new NGUYENLIEU();
                    nlEntity = db.NGUYENLIEUx.Find(id);
                    nlEntity.SoLuongTon = (decimal)nl.SoLuongTon;
                    db.Entry(nlEntity).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    SetAlert("Cập nhật thông tin nguyên liệu thành công", "success");
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    SetAlert("Cập nhật thông tin nguyên liệu thất bại", "error");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(nl);
            }
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(db.NGUYENLIEUx.Where(s => s.NguyenLieu_ID == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Delete(string id, NGUYENLIEU nlEntity)
        {
            try
            {
                nlEntity = db.NGUYENLIEUx.Where(s => s.NguyenLieu_ID == id).FirstOrDefault();
                db.NGUYENLIEUx.Remove(nlEntity);
                db.SaveChanges();
                SetAlert("Xóa thông tin nguyên liệu thành công", "success");
                return RedirectToAction("Index");
            }
            catch
            {
                SetAlert("Xóa thông tin nguyên liệu thất bại", "error");
                return RedirectToAction("Index");
            }
        }
        public ActionResult SelectNCC()
        {   
            NHACUNGCAP se_ncc = new NHACUNGCAP();
            se_ncc.ListNCC = db.NHACUNGCAPs.ToList<NHACUNGCAP>();
            return PartialView(se_ncc);
        }
        public ActionResult SelectDV()
        {
            DONVI se_dv = new DONVI();
            se_dv.ListDV = db.DONVIs.ToList<DONVI>();
            return PartialView(se_dv);
        }
        

    }
}