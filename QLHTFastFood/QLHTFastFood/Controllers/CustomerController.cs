using QLHTFastFood.Code;
using QLHTFastFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHTFastFood.Controllers
{
    public class CustomerController : Controller
    {
        QLFastFoodEntities db = new QLFastFoodEntities();
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detail()
        {
            try
            {
                KHACHHANG kh = Session["KhachHang"] as KHACHHANG;
                DetailKH deKH = new DetailKH();
                deKH.KhachHang_ID = kh.KhachHang_ID;
                deKH.HoTen = kh.HoTen;
                deKH.DiaChi = kh.DiaChi;
                deKH.Email = kh.Email;
                deKH.SDTKH = kh.SDTKH;
                return View(deKH);
            }
            catch
            {
                return RedirectToAction("Login", "CustomerLogin");
            }
        }
        public ActionResult EditInfo(string id)
        {
            try
            {
                KHACHHANG kh = db.KHACHHANGs.Find(id);
                DetailKH deKH = new DetailKH();
                deKH.KhachHang_ID = kh.KhachHang_ID;
                deKH.HoTen = kh.HoTen;
                deKH.DiaChi = kh.DiaChi;
                deKH.Email = kh.Email;
                deKH.SDTKH = kh.SDTKH;
                return View(deKH);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult EditInfo(DetailKH deKH, string id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    KHACHHANG kh = db.KHACHHANGs.Find(id);
                    kh.HoTen = deKH.HoTen;
                    kh.DiaChi = deKH.DiaChi;
                    kh.Email = deKH.Email;
                    kh.SDTKH = deKH.SDTKH;
                    db.Entry(kh).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    Session["KhachHang"] = db.KHACHHANGs.Find(id);
                    return RedirectToAction("Detail");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Cập nhật thất bại, thông tin cập nhật đã tồn tại!");
            }
            return View(deKH);
        }
        public ActionResult DoiMK()
        {
            try
            {
                DoiMK doimk = new DoiMK();
                return View(doimk);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult DoiMK(DoiMK doimk)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    KHACHHANG kh = Session["KhachHang"] as KHACHHANG;
                    if (kh.PassWord != Encryptor.ComputeSha256Hash(doimk.PassWordCu))
                    {
                        ModelState.AddModelError("", "Mật khẩu cũ không trùng khớp!");
                    }
                    else if (doimk.PassWord != doimk.ConfirmPassWord)
                    {
                        ModelState.AddModelError("", "Mật khẩu mới và xác nhận mật khẩu mới không trùng khớp!");
                    }
                    else
                    {
                        KHACHHANG khEntity = db.KHACHHANGs.Find(kh.KhachHang_ID);
                        var encryptedPass = Encryptor.ComputeSha256Hash(doimk.PassWord);
                        khEntity.PassWord = encryptedPass;
                        db.Entry(khEntity).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        Session["KhachHang"] = db.KHACHHANGs.Find(kh.KhachHang_ID);
                        return RedirectToAction("Detail");
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("", "Cập nhật thất bại, lỗi phát sinh!");
            }
            return View(doimk);
        }

        public ActionResult GuiFeedback()
        {
            FEEDBACK fb = new FEEDBACK();
            return View(fb);
        }
        [HttpPost]
        public ActionResult GuiFeedback(FEEDBACK fbEntity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    fbEntity.Feedback_ID = "1";
                    fbEntity.NgayTao = DateTime.Now;
                    db.FEEDBACKs.Add(fbEntity);
                    db.SaveChanges();
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    return View(fbEntity);
                }
            }
            catch
            {
                ModelState.AddModelError("", "Cập nhật thất bại, lỗi phát sinh!");
                return View(fbEntity);
            }
        }
    }
}