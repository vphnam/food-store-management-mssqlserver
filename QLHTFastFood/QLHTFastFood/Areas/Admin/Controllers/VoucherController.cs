using PagedList;
using QLHTFastFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHTFastFood.Areas.Admin.Controllers
{
    public class VoucherController : BaseController
    {
        // GET: Admin/Voucher
        QLFastFoodEntities db = new QLFastFoodEntities();
        public ActionResult Index(string id, int page = 1, int pageSize = 5)
        {
            if (id != null && id != "")
            {
                return View(db.VOUCHERs.Where(x => x.Voucher_ID.StartsWith(id)).OrderBy(x => x.Voucher_ID).ToPagedList(page, pageSize));
            }
            else
            {
                return View(db.VOUCHERs.ToList().OrderBy(n => n.Voucher_ID).ToPagedList(page, pageSize));
            }   
        }
        public ActionResult Create()
        {
            VOUCHER km = new VOUCHER();
            return View(km);
        }
        [HttpPost]
        public ActionResult Create(VOUCHER vcEntity)
        {
            try
            {
                double x = (double)vcEntity.PhanTramKhuyenMai / 100;
                vcEntity.PhanTramKhuyenMai = x;
                if (ModelState.IsValid)
                {
                    if (vcEntity.NgayBatDau >= vcEntity.NgayKetThuc)
                    {
                        ModelState.AddModelError("", "Ngày bắt đầu không được sau ngày kết thúc!!");

                    }
                    if (vcEntity.PhanTramKhuyenMai > 1 || vcEntity.PhanTramKhuyenMai <= 0)
                    {
                        ModelState.AddModelError("", "Phần trăm khuyến mãi chỉ trong phạm vi 0 - 100");

                    }
                    db.VOUCHERs.Add(vcEntity);
                    db.SaveChanges();
                    SetAlert("Tạo mới khuyến mãi thành công", "success");
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                SetAlert("Tạo mới khuyến mãi thất bại. Vui lòng không bỏ trống bất cứ ô nào", "error");
                return View(vcEntity);
            }
            return View(vcEntity);

        }
        public ActionResult Edit(string id)
        {
            VOUCHER km = db.VOUCHERs.Find(id);
            km.PhanTramKhuyenMai = km.PhanTramKhuyenMai * 100;
            return View(km);
        }
        [HttpPost]
        public ActionResult Edit(VOUCHER vcEntity, string id)
        {
            try
            {
                double x = (double)vcEntity.PhanTramKhuyenMai / 100;
                vcEntity.PhanTramKhuyenMai = x;
                if (ModelState.IsValid)
                {
                    if (vcEntity.NgayBatDau >= vcEntity.NgayKetThuc)
                    {
                        ModelState.AddModelError("", "Ngày bắt đầu không được sau ngày kết thúc!!");

                    }
                    if (vcEntity.PhanTramKhuyenMai > 1 || vcEntity.PhanTramKhuyenMai <= 0)
                    {
                        ModelState.AddModelError("", "Phần trăm khuyến mãi chỉ trong phạm vi 0 - 100");

                    }
                    vcEntity.Voucher_ID = id;
                    db.Entry(vcEntity).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    SetAlert("Cập nhật thông tin khuyến mãi thành công", "success");
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                SetAlert("Không được để trống thông tin, cập nhật thất bại!", "error");
                return View(vcEntity);
            }
            return View(vcEntity);

        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(db.VOUCHERs.Where(s => s.Voucher_ID == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Delete(string id, VOUCHER vcEntity)
        {
            try
            {
                vcEntity = db.VOUCHERs.Where(s => s.Voucher_ID == id).FirstOrDefault();
                db.VOUCHERs.Remove(vcEntity);
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