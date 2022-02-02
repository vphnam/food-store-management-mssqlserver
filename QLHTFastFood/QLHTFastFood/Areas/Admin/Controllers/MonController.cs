using PagedList;
using QLHTFastFood.Areas.Admin.Models;
using QLHTFastFood.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHTFastFood.Areas.Admin.Controllers
{
    public class MonController : BaseController
    {
        // GET: Admin/Mon
        QLFastFoodEntities db = new QLFastFoodEntities();
        public ActionResult Index(string id, int page = 1, int pageSize = 30)
        {
           return View(db.MONs.ToList().OrderBy(n => n.Mon_ID).ToPagedList(page, pageSize));
        }
        public ActionResult Create()
        {
            MonEn mon = new MonEn();
            return View(mon);
        }
        [HttpPost]
        public ActionResult Create(MonEn monEntity)
        {
            try
            {
                string filename = Path.GetFileNameWithoutExtension(monEntity.UploadImage.FileName);
                string extent = Path.GetExtension(monEntity.UploadImage.FileName);
                filename = filename + extent;
                monEntity.AnhMon = "~/Images/" + filename;
                monEntity.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Images/"), filename));
                if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Images/"), filename)))
                {
                    ModelState.AddModelError("", "Ảnh đã được sử dụng");
                }
                monEntity.Mon_ID = "1";
                MON mon = new MON();
                mon.Mon_ID = "1";
                mon.TenMon = monEntity.TenMon;
                mon.Loai_ID = monEntity.Loai_ID;
                mon.Size_ID = monEntity.Size_ID;
                mon.DonVi_ID = monEntity.DonVi_ID;
                mon.AnhMon = monEntity.AnhMon;
                mon.DonGia = monEntity.DonGia;
                mon.KhuyenMai_ID = monEntity.KhuyenMai_ID;
                mon.MoTa = monEntity.MoTa;
                db.MONs.Add(mon);
                db.SaveChanges();
                SetAlert("Tạo mới món thành công", "success");
                return RedirectToAction("Index");
            }
            catch
            {
                SetAlert("Tạo mới món thất bại", "error");
                return View(monEntity);
            }
        }
        public ActionResult Edit(string id)
        {
            MON mon = db.MONs.Find(id);
            Session["mon_image"] = mon.AnhMon;

            MonEn monEntity = new MonEn();

            monEntity.Mon_ID = id;
            monEntity.TenMon = mon.TenMon;
            monEntity.Loai_ID = mon.Loai_ID;
            monEntity.Size_ID = mon.Size_ID;
            monEntity.DonVi_ID = mon.DonVi_ID;
            monEntity.AnhMon = mon.AnhMon;
            monEntity.DonGia = mon.DonGia;
            monEntity.KhuyenMai_ID = mon.KhuyenMai_ID;
            monEntity.MoTa = mon.MoTa;

            return View(monEntity);
        }
        [HttpPost]
        public ActionResult Edit(MonEn monEntity, string id)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (monEntity.UploadImage != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(monEntity.UploadImage.FileName);
                        string extent = Path.GetExtension(monEntity.UploadImage.FileName);
                        filename = filename + extent;
                        monEntity.AnhMon = "~/Images/" + filename;
                        monEntity.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Images/"), filename));
                        if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Images/"), filename)))
                        {
                            ModelState.AddModelError("", "Ảnh đã được sử dụng");
                        }
                    }
                    else
                        monEntity.AnhMon = (string)Session["mon_image"];
                    monEntity.Mon_ID = id;
                    MON mon = new MON();
                    mon.Mon_ID = id;
                    mon.TenMon = monEntity.TenMon;
                    mon.Loai_ID = monEntity.Loai_ID;
                    mon.Size_ID = monEntity.Size_ID;
                    mon.DonVi_ID = monEntity.DonVi_ID;
                    mon.AnhMon = monEntity.AnhMon;
                    mon.DonGia = monEntity.DonGia;
                    mon.KhuyenMai_ID = monEntity.KhuyenMai_ID;
                    mon.MoTa = monEntity.MoTa;
                    db.Entry(mon).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    SetAlert("Cập nhật thông tin món thành công", "success");
                    return RedirectToAction("Index");
                }   
                catch
                {
                    SetAlert("Cập nhật thông tin món thất bại", "error");
                    return View(monEntity);
                }
            }
            return View(monEntity);
        }
        public ActionResult EditDonGia(string id)
        {
            MON mon = new MON();
            mon = db.MONs.Find(id);
            MonModel monModel = new MonModel();
            monModel.MaMon = mon.Mon_ID;
            monModel.TenMon = mon.TenMon;
            monModel.DonGia = (int)mon.DonGia;
            monModel.KhuyenMai_ID = mon.KhuyenMai_ID;
            return View(monModel);
        }
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult EditDonGia(MonModel monModel, string id)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    monModel.MaMon = id;
                    MON monEntity = new MON();
                    monEntity =  db.MONs.Find(id);

                    monEntity.DonGia = monModel.DonGia;
                    monEntity.KhuyenMai_ID = monModel.KhuyenMai_ID;

                    db.Entry(monEntity).State = System.Data.Entity.EntityState.Modified;
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
                return View(monModel);
            }
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(db.MONs.Where(s => s.Mon_ID == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Delete(string id, MON monEntity)
        {
            try
            {
                monEntity = db.MONs.Where(s => s.Mon_ID == id).FirstOrDefault();
                db.MONs.Remove(monEntity);
                db.SaveChanges();
                SetAlert("Xóa món thành công", "success");
                return RedirectToAction("Index");
            }
            catch
            {
                SetAlert("Xóa món thất bại", "error");
                return View("Delete", id);
            }
        }
        public ActionResult MonTheoLoai(string id, int page = 1, int pageSize = 10)
        {
            var mon = db.MONs.Where(n => n.Loai_ID == id).ToList();
            var list = mon.OrderBy(n => n.Mon_ID).ToPagedList(page, pageSize);
            return View(list);
        }
        public ActionResult Detail(string id)
        {
            return View(db.MONs.Find(id));
        }
        public ActionResult CongThuc(string id, int page = 1, int pageSize = 10)
        {
            var mon = db.MONs.Find(id);
            ViewBag.MaMon = mon.Mon_ID;
            ViewBag.TenMon = mon.TenMon;
            return View(db.CONGTHUCs.Where(x => x.Mon_ID == id).ToList().OrderBy(n => n.NguyenLieu_ID).ToPagedList(page, pageSize));
        }
        [HttpGet]
        public ActionResult CreateCT(string id)
        {
            CONGTHUC ct = new CONGTHUC();
            ct.Mon_ID = id;
            return View(ct);
        }
        [HttpPost]
        public ActionResult CreateCT(CONGTHUC ctEntity, string id)
        {
            try

            {
                ctEntity.Mon_ID = id;
                db.CONGTHUCs.Add(ctEntity);
                db.SaveChanges();
                SetAlert("Tạo mới công thức thành công", "success");
                return RedirectToAction("CongThuc", new { id = ctEntity.Mon_ID});
            }
            catch
            {
                SetAlert("Tạo mới công thức thất bại", "error");
                return View(ctEntity);
            }
        }
        public ActionResult EditCT(string monID, string nlID)
        {
            CONGTHUC ct = db.CONGTHUCs.Find(monID, nlID);
            return View(ct);
        }
        [HttpPost]
        public ActionResult EditCT(CONGTHUC ct, string monID, string nlID)
        {
            try
            {
                ViewBag.MaMon = monID;
                ct.Mon_ID = monID;
                ct.NguyenLieu_ID = nlID;
                if (ModelState.IsValid)
                {
                    if (ct.SoLuongNLCan <= 0)
                    {
                        ModelState.AddModelError("", "Số lượng nguyên liệu cần phải > 0 !!");
                    }
                    db.Entry(ct).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    SetAlert("Cập nhật công thức thành công", "success");
                    return RedirectToAction("CongThuc", new { id = ct.Mon_ID });
                }
                else
                {
                    return View(ct);
                }
            }
            catch
            {
                SetAlert("Cập nhật công thức thất bại", "error");
                return View(ct);
            }
        }
        [HttpGet]
        public ActionResult DeleteCT(string monID, string nlID)
        {
            return View(db.CONGTHUCs.Where(s => s.Mon_ID == monID && s.NguyenLieu_ID == nlID).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult DeleteCT(CONGTHUC ctEntity,string monID, string nlID)
        {
            try
            {
                ctEntity = db.CONGTHUCs.Where(s => s.Mon_ID == monID && s.NguyenLieu_ID == nlID).FirstOrDefault();
                db.CONGTHUCs.Remove(ctEntity);
                db.SaveChanges();
                SetAlert("Xóa công thức thành công", "success");
                return RedirectToAction("CongThuc", new { id = monID });
            }
            catch
            {
                SetAlert("Xóa công thức thất bại", "error");
                return View("Delete", monID,nlID);
            }
        }
        public ActionResult SelectLoai()
        {
            LOAI se_loai = new LOAI();
            se_loai.ListLoai = db.LOAIs.ToList<LOAI>();
            return PartialView(se_loai);
        }
        public ActionResult SelectDV()
        {
            DONVI se_dv = new DONVI();
            se_dv.ListDV = db.DONVIs.ToList<DONVI>();
            return PartialView(se_dv);
        }
        public ActionResult SelectS()
        {
            SIZE se_s = new SIZE();
            se_s.ListSize = db.SIZEs.ToList<SIZE>();
            return PartialView(se_s);
        }
        public ActionResult SelectKM()
        {
            KHUYENMAI se_km = new KHUYENMAI();
            se_km.ListKM = db.KHUYENMAIs.ToList<KHUYENMAI>();
            return PartialView(se_km);
        }
        public ActionResult SelectNL()
        {
            NGUYENLIEU se_nl = new NGUYENLIEU();
            se_nl.ListNL = db.NGUYENLIEUx.ToList<NGUYENLIEU>();
            return PartialView(se_nl);
        }
    }
}