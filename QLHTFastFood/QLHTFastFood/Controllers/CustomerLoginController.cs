using QLHTFastFood.Areas.Admin.Models;
using QLHTFastFood.Code;
using QLHTFastFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace QLHTFastFood.Controllers
{
    public class CustomerLoginController : Controller
    {
        // GET: CustomerLogin
        QLFastFoodEntities db = new QLFastFoodEntities();
        public ActionResult Index()
        {
            LoginModel login = new LoginModel();
            return View(login);
        }
        [HttpPost]
        public ActionResult Index(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string pass = Encryptor.ComputeSha256Hash(loginModel.PassWord);
                    KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.UserName == loginModel.UserName && n.PassWord == pass);
                    if (kh != null)
                    {

                        if (kh.Status == false)
                        {
                            ModelState.AddModelError("", "Tài khoản đã bị khóa!");
                        }
                        else
                        {
                            Session["KhachHang"] = kh;
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Sai mật khẩu hoặc tài khoản không tồn tại!");
                    }
                }
            }
            catch
            {
                return View(loginModel);
            }
            return View(loginModel);
        }
        public ActionResult Register()
        {
            DangKyKH dk = new DangKyKH();
            return View(dk);
        }
        [HttpPost]
        public ActionResult Register(DangKyKH dkKH)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    KHACHHANG khEntity = new KHACHHANG();
                    khEntity.KhachHang_ID = "1";
                    khEntity.HoTen = dkKH.HoTen;
                    khEntity.UserName = dkKH.UserName;

                    var encryptedPass = Encryptor.ComputeSha256Hash(dkKH.PassWord);
                    khEntity.PassWord = encryptedPass;

                    khEntity.DiaChi = dkKH.DiaChi;
                    khEntity.SDTKH = dkKH.SDTKH;
                    khEntity.Email = dkKH.Email;
                    khEntity.Status = false;
                    db.KHACHHANGs.Add(khEntity);
                    db.SaveChanges();
                    KHACHHANG kh = db.KHACHHANGs.Where(x => x.Email == khEntity.Email).FirstOrDefault();
                    BuildEmailTemplateForRegister(kh.KhachHang_ID);
                    return RedirectToAction("ConfirmDoiMK");
                }
                else
                {
                    return View(dkKH);
                }
            }
            catch
            {
                ModelState.AddModelError("", "Thông tin vừa cập nhật đã được đăng ký trước. Vui lòng kiểm tra!");
                return View(dkKH);
            }
        }
        public ActionResult XacNhanTrangThai(string maKH)
        {
            KHACHHANG kh = db.KHACHHANGs.Find(maKH);
            if (kh.Status == false)
            {
                kh.Status = true;
                db.Entry(kh).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public void BuildEmailTemplateForRegister(string MaKH)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/Assets/template/") + "DangKyKH" + ".cshtml");
            var kh = db.KHACHHANGs.Where(x => x.KhachHang_ID == MaKH).FirstOrDefault();
            /*var url = "http://fastfoodnumone.somee.com/" + "CustomerLogin/XacNhanTrangThai?maKH=" + kh.KhachHang_ID;*/
            var url = "http://localhost:57687/" + "CustomerLogin/XacNhanTrangThai?maKH=" + kh.KhachHang_ID;
            body = body.Replace("@ViewBag.LinkXacNhan", url);
            body = body.Replace("@ViewBag.HoTenKH", kh.HoTen);
            body = body.Replace("@ViewBag.SDT", kh.SDTKH);
            body = body.Replace("@ViewBag.DiaChi", kh.DiaChi);
            body = body.ToString();
            GuiEmail("Thông báo đăng ký tài khoản", body, kh.Email);
        }
        public ActionResult QuenMatKhau()
        {
            return View();
        }
        [HttpPost]
        public ActionResult QuenMatKhau(FormCollection collection)
        {
            string email = (string)collection["Email"];
            if (string.IsNullOrEmpty(email))
            {
                ViewData["Loi1"] = "Không được để trống email";
            }
            else
            {
                try
                {
                    KHACHHANG kh = db.KHACHHANGs.Where(x => x.Email == email).FirstOrDefault();
                    if(kh == null)
                    {
                        ViewData["Loi2"] = "Email không tồn tại";
                    }
                    else if(kh != null)
                    {
                        BuildEmailTemplate(kh.KhachHang_ID);
                        return RedirectToAction("ConfirmDoiMK");
                    }
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }
        public ActionResult Chuyen(string Email)
        {
            if(Session["Email"] == null)
            {
                KHACHHANG kh = db.KHACHHANGs.Where(x => x.Email == Email).FirstOrDefault();
                string baomat = CreatePassword(6);
                Session["MaBaoMat"] = baomat;
                BuildEmailTemplateForPassword(kh.KhachHang_ID, baomat);
                return RedirectToAction("ForgetPassword", new { email = Email });
            }
            else
            {
                Email = (string)Session["Email"];
                KHACHHANG kh = db.KHACHHANGs.Where(x => x.Email == Email).FirstOrDefault();
                string baomat = CreatePassword(6);
                Session["MaBaoMat"] = baomat;
                BuildEmailTemplateForPassword(kh.KhachHang_ID, baomat);
                return RedirectToAction("ForgetPassword", new { email = Email });
            }
        }
        public ActionResult ForgetPassword(string email)
        {
            Session["Email"] = email;
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(FormCollection collection, string email)
        {
            string mk = (string)collection["MatKhau"];
            string cfmk = (string)collection["ConfirmMatKhau"];
            string inputmaBaoMat = (string)collection["maBaoMat"];
            string maBaoMat = (string)Session["MaBaoMat"];
            if (string.IsNullOrEmpty(mk))
            {
                ViewData["Loi1"] = "Không được để trống mật khẩu";
            }
            else if (string.IsNullOrEmpty(cfmk))
            {
                ViewData["Loi2"] = "Không được để trống xác nhận mật khẩu";
            }
            else if(string.IsNullOrEmpty(inputmaBaoMat))
            {
                ViewData["Loi4"] = "Không được để trống mã bảo mật";
            }
            else
            {
                if(mk != cfmk)
                {
                    ViewData["Loi3"] = "Mật khẩu và xác nhận mật khẩu không giống nhau";
                }
                else if(maBaoMat != inputmaBaoMat)
                {
                    ViewData["Loi5"] = "Mã bảo mật không chính xác";
                }
                else
                {
                    try
                    {
                        KHACHHANG kh = db.KHACHHANGs.Where(x => x.Email == email).FirstOrDefault();
                        string encryptedPass = Encryptor.ComputeSha256Hash(mk);
                        kh.PassWord = encryptedPass;
                        db.Entry(kh).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        return View();
                    }
                }      
            }
            return View();
        }
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        public ActionResult ConfirmDoiMK()
        {
            return View();
        }
        public void BuildEmailTemplate(string MaKH)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/Assets/template/") + "FormDoiMK" + ".cshtml");
            var kh = db.KHACHHANGs.Where(x => x.KhachHang_ID == MaKH).FirstOrDefault();
            /*var url = "http://fastfoodnumone.somee.com/" + "CustomerLogin/Chuyen?email=" + kh.Email;*/
            var url = "http://localhost:57687/" + "CustomerLogin/Chuyen?email=" + kh.Email;
            body = body.Replace("@ViewBag.LinkXacNhan", url);
            body = body.Replace("@ViewBag.HoTenKH", kh.HoTen);
            body = body.Replace("@ViewBag.SDT", kh.SDTKH);
            body = body.ToString();
            GuiEmail("Thông báo đổi mật khẩu", body, kh.Email);
        }
        public void BuildEmailTemplateForPassword(string MaKH,string ranPass)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/Assets/template/") + "MaBaoMat" + ".cshtml");
            var kh = db.KHACHHANGs.Where(x => x.KhachHang_ID == MaKH).FirstOrDefault();
            body = body.Replace("@ViewBag.MaBaoMat", ranPass);
            body = body.ToString();
            GuiEmail("Thông báo về mã bảo mật", body, kh.Email);
        }
        public void GuiEmail(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            from = "hoainam11134@gmail.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendMail(mail);
        }

        public static void SendMail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("hoainam11134@gmail.com", "hoainam0977529557asd161120");
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Logout()
        {
            Session["KhachHang"] = null;
            return RedirectToAction("Index","Home");
        }
    }
}