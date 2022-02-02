using PagedList;
using QLHTFastFood.Areas.Admin.Models;
using QLHTFastFood.Code;
using QLHTFastFood.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHTFastFood.Areas.Admin.Controllers
{
    public class NhanVienController : BaseController
    {
        QLFastFoodEntities db = new QLFastFoodEntities();
        // GET: Admin/NhanVien
        public ActionResult Index(string id, int page = 1, int pageSize = 10)
        {
            if (id != null && id != "")
            {
                return View(db.NHANVIENs.Where(x => x.NhanVien_ID.StartsWith(id)).OrderBy(x => x.NhanVien_ID).ToPagedList(page, pageSize));
            }
            else
            {
                return View(db.NHANVIENs.ToList().OrderBy(n => n.NhanVien_ID).ToPagedList(page, pageSize));
            }   
        }
        public ActionResult Detail(string id)
        {
            NHANVIEN nv = db.NHANVIENs.Find(id);
            Session["NV"] = nv;
            return View(nv);
        }
        public ActionResult Create()
        {
            CreateNVModel createNVModel = new CreateNVModel();
            return View(createNVModel);
        }
        [HttpPost]
        public ActionResult Create(CreateNVModel createNVModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultUN = db.NHANVIENs.SingleOrDefault(x => x.UserName == createNVModel.UserName);
                    var resultSDT = db.NHANVIENs.SingleOrDefault(x => x.SDT == createNVModel.SDT);
                    var resultEmail = db.NHANVIENs.SingleOrDefault(x => x.Email == createNVModel.Email);
                    if (resultUN != null)
                    {
                        ModelState.AddModelError("", "UserName đã bị trùng");
                    }
                    else if (resultSDT != null)
                    {
                        ModelState.AddModelError("", "Số điện thoại đã bị trùng");
                    }
                    else if (createNVModel.PassWord != createNVModel.ConfirmPassword)
                    {
                        ModelState.AddModelError("", "Mật khẩu và xác nhận mật khẩu không giống nhau!");
                    }
                    else if (resultEmail != null)
                    {
                        ModelState.AddModelError("", "Email đã bị trùng");
                    }
                    else
                    {
                        NHANVIEN nvEntity = new NHANVIEN();
                        nvEntity.NhanVien_ID = "1";
                        nvEntity.HoTenNV = createNVModel.HoTenNV;
                        nvEntity.UserName = createNVModel.UserName;

                        var encryptedPass = Encryptor.ComputeSha256Hash(createNVModel.PassWord);
                        nvEntity.PassWord = encryptedPass;

                        nvEntity.Quyen_ID = createNVModel.Quyen_ID;
                        nvEntity.NgaySinh = createNVModel.NgaySinh;
                        nvEntity.DiaChi = createNVModel.DiaChi;
                        nvEntity.SDT = createNVModel.SDT;
                        nvEntity.Email = createNVModel.Email;
                        nvEntity.Status = true;
                        db.NHANVIENs.Add(nvEntity);
                        db.SaveChanges();
                        SetAlert("Tạo mới nhân viên thành công", "success");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View(createNVModel);
                }
            }
            catch
            {
                SetAlert("Tạo mới nhân viên thất bại", "error");
                return View(createNVModel);
            }
            return View(createNVModel);
        }
        public ActionResult Edit(string id)
        {
            NHANVIEN nv = db.NHANVIENs.Find(id);
            EditNVModel editnv = new EditNVModel();
            editnv.HoTenNV = nv.HoTenNV;
            editnv.Quyen_ID = nv.Quyen_ID;
            editnv.NgaySinh = nv.NgaySinh;
            editnv.SDT = nv.SDT;
            editnv.DiaChi = nv.DiaChi;
            editnv.Email = nv.Email;
            return View(editnv);
        }
        [HttpPost]
        public ActionResult Edit(EditNVModel editNV, string id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NHANVIEN nvEntity = new NHANVIEN();
                    NHANVIEN nv = (NHANVIEN)Session["NV"];
                    nvEntity.NhanVien_ID = id;
                    nvEntity.HoTenNV = editNV.HoTenNV;
                    nvEntity.Quyen_ID = editNV.Quyen_ID;
                    nvEntity.NgaySinh = editNV.NgaySinh;
                    nvEntity.SDT = editNV.SDT;
                    nvEntity.DiaChi = editNV.DiaChi;
                    nvEntity.Email = editNV.Email;
                    nvEntity.UserName = nv.UserName;
                    nvEntity.PassWord = nv.PassWord;
                    nvEntity.Status = nv.Status;
                    db.Entry(nvEntity).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    SetAlert("Cập nhật thông tin nhân viên thành công", "success");
                    return RedirectToAction("Index");
                }
                return View(editNV);
            }
            catch
            {
                SetAlert("Cập nhật thông tin nhân viên thất bại", "error");
                return RedirectToAction("Index");
            }
            
        }
        public ActionResult EditMK(string id)
        {
            EditMK editMK = new EditMK();
            return View(editMK);
        }
        [HttpPost]
        public ActionResult EditMK(EditMK editMK, string id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NHANVIEN nvEntity = new NHANVIEN();
                    NHANVIEN nv = (NHANVIEN)Session["NV"];

                    if(editMK.NewPassWord != editMK.ConfirmNewPassword)
                    {
                        ModelState.AddModelError("", "Mật khẩu mới và nhập lại mật khẩu mới không chính xác");
                    }
                    else
                    {
                        nvEntity.NhanVien_ID = id;
                        nvEntity.HoTenNV = nv.HoTenNV;
                        nvEntity.Quyen_ID = nv.Quyen_ID;
                        nvEntity.NgaySinh = nv.NgaySinh;
                        nvEntity.SDT = nv.SDT;
                        nvEntity.DiaChi = nv.DiaChi;
                        nvEntity.Email = nv.Email;
                        nvEntity.UserName = nv.UserName;
                        nvEntity.PassWord = Encryptor.ComputeSha256Hash(editMK.NewPassWord);
                        nvEntity.Status = nv.Status;
                        db.Entry(nvEntity).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        SetAlert("Cập nhật thông tin nhân viên thành công", "success");
                        return RedirectToAction("Index");
                    }                    
                }
                return View(editMK);
            }
            catch
            {
                SetAlert("Cập nhật thông tin nhân viên thất bại", "error");
                return RedirectToAction("Index");
            }

        }
        public ActionResult LockAccount(string id)
        {
            NHANVIEN nv = db.NHANVIENs.Find(id);
            nv.Status = false;
            db.Entry(nv).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            SetAlert("Đã khóa tài khoản nhân viên thành công", "success");
            return RedirectToAction("Index");
        }
        public ActionResult OpenAccount(string id)
        {
            NHANVIEN nv = db.NHANVIENs.Find(id);
            nv.Status = true;
            db.Entry(nv).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            SetAlert("Đã mở khóa tài khoản nhân viên thành công", "success");
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(db.NHANVIENs.Where(s => s.NhanVien_ID == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Delete(string id, NHANVIEN nvEntity)
        {
            try
            {
                nvEntity = db.NHANVIENs.Where(s => s.NhanVien_ID == id).FirstOrDefault();
                db.NHANVIENs.Remove(nvEntity);
                db.SaveChanges();
                SetAlert("Xóa thông tin nhân viên thành công", "success");
                return RedirectToAction("Index");
            }
            catch
            {
                SetAlert("Xóa thông tin nhân viên thất bại", "error");
                return View("Delete", id);
            }
        }
        public ActionResult SelectQ()
        {
            QUYENTRUYCAP se_q = new QUYENTRUYCAP();
            se_q.ListQ = db.QUYENTRUYCAPs.ToList<QUYENTRUYCAP>();
            return PartialView(se_q);
        }
    }
}