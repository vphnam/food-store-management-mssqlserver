using QLHTFastFood.Areas.Admin.Models;
using QLHTFastFood.Code;
using QLHTFastFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHTFastFood.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        QLFastFoodEntities db = new QLFastFoodEntities();
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoginAdmin()
        {
            if (Session["NhanVienAdmin"] == null)
            {
                LoginModel login = new LoginModel();
                return View(login);
            }
            else
                return RedirectToAction("Index", "Admin");
        }
        [HttpPost]
        public ActionResult LoginAdmin(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                string pass = Encryptor.ComputeSha256Hash(loginModel.PassWord);
                NHANVIEN nv = db.NHANVIENs.SingleOrDefault(n => n.UserName == loginModel.UserName && n.PassWord == pass);
                if (nv != null)
                {
                    if (nv.Quyen_ID != "Q001" && nv.Quyen_ID != "Q002" && nv.Quyen_ID != "Q003")
                    {
                        ModelState.AddModelError("", "Tài khoản không có quyền truy cập vào trang!");
                    }
                    else if (nv.Status == false)
                    {
                        ModelState.AddModelError("", "Tài khoản đã bị khóa!");
                    }
                    else
                    {
                        Session["TenNV"] = nv.HoTenNV;
                        Session["Quyen"] = nv.QUYENTRUYCAP.TenQuyen;
                        Session["NhanVienAdmin"] = nv;
                        return RedirectToAction("Index", "Admin");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại!");
                }
            }
            return View();
        }
        
        public ActionResult Logout()
        {
            Session["NhanVienAdmin"] = null;
            return RedirectToAction("LoginAdmin");
        }
    }
}