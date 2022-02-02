using PagedList;
using QLHTFastFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLHTFastFood.Areas.Admin.Models;
using QLHTFastFood.Code;

namespace QLHTFastFood.Areas.Admin.Controllers
{
    public class KhachHangController : BaseController
    {
        // GET: Admin/KhachHang
        QLFastFoodEntities db = new QLFastFoodEntities();
        public ActionResult Index(string id, int page = 1, int pageSize = 10)
        {
            if(id != null && id != "")
            {
                List<KHACHHANG> lstKH = db.KHACHHANGs.Where(x => x.KhachHang_ID.StartsWith(id)).ToList();
                if(lstKH.Count > 0)
                {
                    return View(db.KHACHHANGs.Where(x => x.KhachHang_ID.StartsWith(id)).OrderBy(x => x.KhachHang_ID).ToPagedList(page, pageSize));
                }
                else
                {
                    return View(db.KHACHHANGs.Where(x => x.HoTen.StartsWith(id)).OrderBy(x => x.KhachHang_ID).ToPagedList(page, pageSize));
                }        
            }
            else
            {
                return View(db.KHACHHANGs.ToList().OrderBy(n => n.KhachHang_ID).ToPagedList(page, pageSize));
            }       
        }
        public ActionResult Detail(string id)
        {
            KHACHHANG nv = db.KHACHHANGs.Find(id);
            return View(nv);
        }
        public ActionResult Create()
        {
            CreateKHModel creKHModel = new CreateKHModel();
            return View(creKHModel);
        }
        [HttpPost]
        public ActionResult Create(CreateKHModel creKHModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultUN = db.KHACHHANGs.SingleOrDefault(x => x.UserName == creKHModel.UserName);
                    var resultSDT = db.KHACHHANGs.SingleOrDefault(x => x.SDTKH == creKHModel.SDTKH);
                    var resultEmail = db.KHACHHANGs.SingleOrDefault(x => x.Email == creKHModel.Email);
                    if (resultUN != null)
                    {
                        ModelState.AddModelError("", "UserName đã bị trùng");
                    }
                    else if (resultSDT != null)
                    {
                        ModelState.AddModelError("", "Số điện thoại đã bị trùng");
                    }
                    else if (creKHModel.PassWord != creKHModel.ConfirmPassword)
                    {
                        ModelState.AddModelError("", "Mật khẩu và xác nhận mật khẩu không giống nhau!");
                    }
                    else if (resultEmail != null)
                    {
                        ModelState.AddModelError("", "Email đã bị trùng");
                    }
                    else
                    {
                        KHACHHANG khEntity = new KHACHHANG();
                        khEntity.KhachHang_ID = "1";
                        khEntity.HoTen = creKHModel.HoTen;
                        khEntity.UserName = creKHModel.UserName;

                        var encryptedPass = Encryptor.ComputeSha256Hash(creKHModel.PassWord);
                        khEntity.PassWord = encryptedPass;

                        khEntity.DiaChi = creKHModel.DiaChi;
                        khEntity.SDTKH = creKHModel.SDTKH;
                        khEntity.Email = creKHModel.Email;
                        khEntity.Status = true;
                        db.KHACHHANGs.Add(khEntity);
                        db.SaveChanges();
                        SetAlert("Tạo mới khách hàng thành công", "success");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View(creKHModel);
                }
            }
            catch
            {
                SetAlert("Tạo mới khách hàng thất bại", "error");
                return View(creKHModel);
            }
            return View(creKHModel);
        }
        public ActionResult LockAccount(string id)
        {
            KHACHHANG nv = db.KHACHHANGs.Find(id);
            nv.Status = false;
            db.Entry(nv).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            SetAlert("Đã khóa tài khoản khách hàng thành công", "success");
            return RedirectToAction("Index");
        }
        public ActionResult OpenAccount(string id)
        {
            KHACHHANG nv = db.KHACHHANGs.Find(id);
            nv.Status = true;
            db.Entry(nv).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            SetAlert("Đã mở khóa tài khoản khách hàng thành công", "success");
            return RedirectToAction("Index");
        }
    }
}