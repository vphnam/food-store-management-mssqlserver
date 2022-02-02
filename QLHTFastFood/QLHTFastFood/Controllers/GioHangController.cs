using QLHTFastFood.Models;
using QLHTFastFood.NganLuongAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLHTFastFood.Controllers
{
    [HandleError]
    public class GioHangController : Controller
    {
        // GET: GioHang
        QLFastFoodEntities db = new QLFastFoodEntities();
        public ActionResult Index()
        {
            return View();
        }
        public List<GioHang> Laygiohang()
        {
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<GioHang>();
                Session["GioHang"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGioHang(string maMon, int soLuong, string strURL)
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.Find(n => n.maMon == maMon);
            int i = 0;

            List<CONGTHUC> lstCT = db.CONGTHUCs.Where(x => x.Mon_ID == maMon).ToList();
            List<NGUYENLIEU> lstNL = Session["NguyenLieu"] as List<NGUYENLIEU>;
            foreach(var item in lstCT)
            {
                if(lstNL.FirstOrDefault(s => s.NguyenLieu_ID == item.NguyenLieu_ID).SoLuongTon > 0)
                {
                    double kiemTra = (double)lstNL.FirstOrDefault(s => s.NguyenLieu_ID == item.NguyenLieu_ID).SoLuongTon - (double)(item.SoLuongNLCan * soLuong);
                    if (kiemTra < 0)
                    {
                        i++;
                    }
                }
                else
                {
                    i++;
                }

            }    
            if(i == 0)
            {
                TempData["HetHang"] = "Thêm món thành công";
                TempData["Type"] = "success";
                foreach(var item in lstCT)
                {
                    lstNL.FirstOrDefault(s => s.NguyenLieu_ID == item.NguyenLieu_ID).SoLuongTon = lstNL.FirstOrDefault(s => s.NguyenLieu_ID == item.NguyenLieu_ID).SoLuongTon - (item.SoLuongNLCan * soLuong);
                }
                Session["NguyenLieu"] = lstNL;
                if (sanpham == null)
                {
                    sanpham = new GioHang(maMon, soLuong);
                    lstGiohang.Add(sanpham);
                    int x = lstGiohang.Sum(n => n.soLuong);
                    Session["SoLuong"] = x;
                    return Redirect(strURL);
                }
                else
                {
                    sanpham.soLuong = sanpham.soLuong + soLuong;
                    int x = lstGiohang.Sum(n => n.soLuong);
                    Session["SoLuong"] = x;
                    return Redirect(strURL);
                }
            }
            else
            {
                TempData["HetHang"] = "Món vừa chọn đã hết, vui lòng chọn món khác";
                TempData["Type"] = "error";
                return Redirect(strURL);
            }
                

        }
        private int TongSoLuong()
        {
            int TongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                TongSoLuong = lstGioHang.Sum(n => n.soLuong);
            }
            return TongSoLuong;
        }
        private double TongTien()
        {
            double TongTien = 0;
            List<GioHang> lstGiohang = Session["Giohang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                TongTien = lstGiohang.Sum(n => n.thanhTien);
            }
            return TongTien;
        }
        private double TongTienKM(string maVC, string maDH)
        {
            double TongTien = 0;
            double TongKM = 0;
            if (maVC == null)
            {
                TongTien = (double)db.CTDONHANGs.Where(x => x.DonHang_ID == maDH).Sum(x => x.ThanhTien);
                return TongTien;
            }
            else
            {
                TongTien = (double)db.CTDONHANGs.Where(x => x.DonHang_ID == maDH).Sum(x => x.ThanhTien);
                VOUCHER vc = db.VOUCHERs.Find(maVC);
                double TienKM = TongTien - TongTien * (double)vc.PhanTramKhuyenMai;
                if (TienKM > (double)vc.GiamGiaToiDa)
                {
                    TongKM = TongTien - (double)vc.GiamGiaToiDa;
                }
                else
                {
                    TongKM = TongTien - TienKM;
                }
                return TongKM;
            }
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("GioHangTrong");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGiohang);
        }
        public ActionResult GioHangTrong()
        {
            return View();
        }
        public ActionResult XoaGiohang(string maMon)
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.maMon == maMon);
            if (sanpham != null)
            {
                List<CONGTHUC> lstCT = db.CONGTHUCs.Where(x => x.Mon_ID == maMon).ToList();
                List<NGUYENLIEU> lstNL = Session["NguyenLieu"] as List<NGUYENLIEU>;
                foreach (var item in lstCT)
                {
                    lstNL.FirstOrDefault(s => s.NguyenLieu_ID == item.NguyenLieu_ID).SoLuongTon = lstNL.FirstOrDefault(s => s.NguyenLieu_ID == item.NguyenLieu_ID).SoLuongTon + (item.SoLuongNLCan * sanpham.soLuong);
                }
                Session["NguyenLieu"] = lstNL;
                lstGiohang.RemoveAll(n => n.maMon == maMon);
                int sl = lstGiohang.Sum(n => n.soLuong);
                Session["SoLuong"] = sl;
                TempData["HetHang"] = "Xóa món thành công";
                TempData["Type"] = "success";
                return RedirectToAction("GioHang");
            }
            if (lstGiohang.Count == 0)
                return RedirectToAction("Index", "Home");
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGiohang()
        {
            List<GioHang> lstGioHang = Laygiohang();
            Session["NguyenLieu"] = db.NGUYENLIEUx.ToList();
            lstGioHang.Clear();
            Session["SoLuong"] = 0;
            TempData["HetHang"] = "Xóa tất cả món trong giỏ hàng thành công, giỏ hàng trống";
            TempData["Type"] = "success";
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang(string maMon, FormCollection collection)
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.maMon == maMon);
            if (sanpham != null)
            {

                int i = 0;
                int soLuongTruRa;
                int sLTrongGio = lstGiohang.FirstOrDefault(x => x.maMon == maMon).soLuong;
                int sLDat = int.Parse(collection["txtSoLuong"].ToString());
                if(sLTrongGio == sLDat)
                {
                    TempData["HetHang"] = "Số lượng cập nhật và trong giỏ bằng nhau, không có thay đổi";
                    TempData["Type"] = "success";
                    return RedirectToAction("Giohang");
                }
                else
                {
                    soLuongTruRa = Math.Abs(sLDat - sLTrongGio);
                }
                if(sLDat > sLTrongGio)
                {
                    List<CONGTHUC> lstCT = db.CONGTHUCs.Where(x => x.Mon_ID == maMon).ToList();
                    List<NGUYENLIEU> lstNL = Session["NguyenLieu"] as List<NGUYENLIEU>;
                    foreach (var item in lstCT)
                    {
                        if (lstNL.FirstOrDefault(s => s.NguyenLieu_ID == item.NguyenLieu_ID).SoLuongTon > 0)
                        {
                            double kiemTra = (double)lstNL.FirstOrDefault(s => s.NguyenLieu_ID == item.NguyenLieu_ID).SoLuongTon - (double)(item.SoLuongNLCan * soLuongTruRa);
                            if (kiemTra < 0)
                            {
                                i++;
                            }
                        }
                        else
                        {
                            i++;
                        }
                    }
                    if (i == 0)
                    {
                        TempData["HetHang"] = "Cập nhật món thành công";
                        TempData["Type"] = "success";
                        foreach (var item in lstCT)
                        {
                            lstNL.FirstOrDefault(s => s.NguyenLieu_ID == item.NguyenLieu_ID).SoLuongTon = lstNL.FirstOrDefault(s => s.NguyenLieu_ID == item.NguyenLieu_ID).SoLuongTon - (item.SoLuongNLCan * soLuongTruRa);
                        }
                        Session["NguyenLieu"] = lstNL;
                        if (sanpham == null)
                        {
                            sanpham = new GioHang(maMon, sLDat);
                            lstGiohang.Add(sanpham);
                            int x = lstGiohang.Sum(n => n.soLuong);
                            Session["SoLuong"] = x;
                        }
                        else
                        {
                            sanpham.soLuong = sanpham.soLuong + soLuongTruRa;
                            int x = lstGiohang.Sum(n => n.soLuong);
                            Session["SoLuong"] = x;
                        }
                    }
                    else
                    {
                        TempData["HetHang"] = "Không đủ món, cập nhật thất bại";
                        TempData["Type"] = "error";
                    }
                }
                else
                {
                    TempData["HetHang"] = "Cập nhật món thành công";
                    TempData["Type"] = "success";
                    List<CONGTHUC> lstCT = db.CONGTHUCs.Where(x => x.Mon_ID == maMon).ToList();
                    List<NGUYENLIEU> lstNL = Session["NguyenLieu"] as List<NGUYENLIEU>;

                    foreach (var item in lstCT)
                    {
                        lstNL.FirstOrDefault(s => s.NguyenLieu_ID == item.NguyenLieu_ID).SoLuongTon = lstNL.FirstOrDefault(s => s.NguyenLieu_ID == item.NguyenLieu_ID).SoLuongTon + (item.SoLuongNLCan * soLuongTruRa);
                    }
                    Session["NguyenLieu"] = lstNL;
                    if (sanpham == null)
                    {
                        sanpham = new GioHang(maMon, sLDat);
                        lstGiohang.Add(sanpham);
                        int x = lstGiohang.Sum(n => n.soLuong);
                        Session["SoLuong"] = x;
                    }
                    else
                    {
                        sanpham.soLuong = sanpham.soLuong - soLuongTruRa;
                        int x = lstGiohang.Sum(n => n.soLuong);
                        Session["SoLuong"] = x;
                    }
                }
            }
            return RedirectToAction("Giohang");
        }

        public ActionResult DatHang()
        {
            if (Session["KhachHang"] == null)
                return RedirectToAction("Index", "CustomerLogin");
            List<GioHang> lstGiohang = Laygiohang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGiohang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            DONHANG dh = new DONHANG();
            KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
            DateTime today = DateTime.Now;
            List<GioHang> gh = Laygiohang();
            string phuongthuc = collection["paymentMethod"];
            var ngaygiao = String.Format("{0:dd/MM/yyyy}", collection["NgayGiao"]);
            if (string.IsNullOrEmpty(ngaygiao))
            {
                ViewData["Loi1"] = "Ngày giao không được trống";
            }        
            else
            {
                DateTime giao = DateTime.Parse(collection["NgayGiao"]);
                System.TimeSpan ngaydatru = giao.Subtract(today);
                int ngay = ngaydatru.Days;
                if (ngay > 3)
                {
                    ViewData["Loi2"] = "Không được đặt hàng trước quá 3 ngày!";
                }
                else if(ngay < 0)
                {
                    ViewData["Loi5"] = "Ngày giao hàng phải sau ngày đặt hàng!";
                }
                else if(ngay == 0)
                {
                    int phut = ngaydatru.Minutes;
                    if (phut <= 30)
                    {
                        ViewData["Loi6"] = "Thời gian giao phải tối thiểu 30 phút!";
                    }
                }
                else if (collection["Voucher"] != null)
                {
                    string mavc = (string)collection["Voucher"];
                    VOUCHER vc = db.VOUCHERs.Where(x => x.Voucher_ID == mavc).FirstOrDefault();
                    
                    if (vc != null && mavc != "")
                    {  
                        if (vc.NgayBatDau <= today && today >= vc.NgayKetThuc)
                        {
                            ViewData["Loi4"] = "Thời gian sử dụng voucher đã không còn hợp lệ";
                        }
                    }
                    if (phuongthuc == "CASH")
                    {
                        try
                        {
                            dh.DonHang_ID = "1";
                            dh.KhachHang_ID = kh.KhachHang_ID;
                            dh.NhanVien_ID = null;
                            dh.Loai = false;
                            dh.DaXacNhan = false;
                            dh.DaThanhToan = false;
                            dh.ThoiGianDat = DateTime.Now;
                            dh.ThoiGianGiao = DateTime.Parse(ngaygiao);
                            if (mavc != null && mavc != "")
                            {
                                dh.Voucher_ID = mavc;
                            }
                            else
                                dh.Voucher_ID = null;
                            db.DONHANGs.Add(dh);
                            db.SaveChanges();
                            dh = db.DONHANGs.OrderByDescending(x => x.DonHang_ID).FirstOrDefault();
                            Session["DonHang"] = dh;
                            foreach (var item in gh)
                            {
                                CTDONHANG ctdh = new CTDONHANG();
                                ctdh.DonHang_ID = dh.DonHang_ID;
                                ctdh.Mon_ID = item.maMon;
                                ctdh.SoLuongMua = item.soLuong;
                                ctdh.DonGiaMua = (decimal)item.donGia;
                                ctdh.ThanhTien = (decimal)item.thanhTien;
                                db.CTDONHANGs.Add(ctdh);
                                db.SaveChanges();
                            }
                            Session["GioHang"] = null;
                            Session["SoLuong"] = null;
                            double tongtien;
                            if (mavc != null && mavc != "")
                            {
                                tongtien = TongTienKM(mavc, dh.DonHang_ID);
                            }
                            else
                                 tongtien = (double)db.CTDONHANGs.Where(x => x.DonHang_ID == dh.DonHang_ID).Sum(x => x.ThanhTien);
                            BuildEmailTemplate(dh.DonHang_ID, kh.KhachHang_ID, tongtien);

                            return RedirectToAction("XacNhanDonHang", "Giohang");

                        }
                        catch
                        {
                            return View(gh);
                        }
                    }
                    else
                    {
                        try
                        {
                            dh.DonHang_ID = "1";
                            dh.KhachHang_ID = kh.KhachHang_ID;
                            dh.NhanVien_ID = null;
                            dh.Loai = false;
                            dh.DaXacNhan = false;
                            dh.DaThanhToan = false;
                            dh.ThoiGianDat = DateTime.Now;
                            dh.ThoiGianGiao = DateTime.Parse(ngaygiao);
                            if (mavc != null && mavc != "")
                            {
                                dh.Voucher_ID = mavc;
                            }
                            else
                                dh.Voucher_ID = null;
                            db.DONHANGs.Add(dh);
                            db.SaveChanges();
                            dh = db.DONHANGs.OrderByDescending(x => x.DonHang_ID).FirstOrDefault();
                            Session["DonHang"] = dh;
                            foreach (var item in gh)
                            {
                                CTDONHANG ctdh = new CTDONHANG();
                                ctdh.DonHang_ID = dh.DonHang_ID;
                                ctdh.Mon_ID = item.maMon;
                                ctdh.SoLuongMua = item.soLuong;
                                ctdh.DonGiaMua = (decimal)item.donGia;
                                ctdh.ThanhTien = (decimal)item.thanhTien;
                                db.CTDONHANGs.Add(ctdh);
                                db.SaveChanges();
                            }
                            double tongtien;
                            if (mavc != null && mavc != "")
                            {
                                tongtien = TongTienKM(mavc, dh.DonHang_ID);
                            }
                            else
                                tongtien = (double)db.CTDONHANGs.Where(x => x.DonHang_ID == dh.DonHang_ID).Sum(x => x.ThanhTien);

                            string bankcode = collection["bankcode"];
                            var merchantId = ConfigurationManager.AppSettings["MerchantId"].ToString();
                            var merchantPassword = ConfigurationManager.AppSettings["MerchantPassword"].ToString();
                            var merchantEmail = ConfigurationManager.AppSettings["MerchantEmail"].ToString();

                            RequestInfo info = new RequestInfo();
                            info.Merchant_id = merchantId;
                            info.Merchant_password = merchantPassword;
                            info.Receiver_email = merchantEmail;



                            info.cur_code = "vnd";
                            info.bank_code = bankcode;

                            info.Order_code = dh.DonHang_ID;
                            info.Total_amount = tongtien.ToString();
                            info.fee_shipping = "0";
                            info.Discount_amount = "0";
                            info.order_description = "Thanh toán đơn hàng tại FastFoodNumOne";
                            info.return_url = "http://localhost:57687/" + "GioHang/ConfirmDH";
                            info.cancel_url = "http://localhost:57687/" + "GioHang/CancelDatHang?id=" + dh.DonHang_ID.ToString();

                            info.Buyer_fullname = kh.HoTen;
                            info.Buyer_email = kh.Email;
                            info.Buyer_mobile = kh.SDTKH;

                            APICheckoutV3 objNLChecout = new APICheckoutV3();
                            ResponseInfo result = objNLChecout.GetUrlCheckout(info, phuongthuc);
                            if (result.Error_code == "00")
                            {
                                Session["GioHang"] = null;
                                Session["SoLuong"] = null;
                                return Redirect(result.Checkout_url);                                               
                            }
                            else
                            {
                                return RedirectToAction("CancelDatHang", new { id = dh.DonHang_ID, mess = result.Description});
                            }
                        }
                        catch(Exception ex)
                        {
                            return View(gh);
                        }

                    }
                }
                

            }
            return View(gh);
        }
        public ActionResult CancelDatHang(string id, string mess)
        {
            try
            {   
                Request.Url.AbsoluteUri.Replace(Request.Url.Query, String.Empty);
                List<CTDONHANG> lstDH = db.CTDONHANGs.Where(x => x.DonHang_ID == id).ToList();
                foreach (var item in lstDH)
                {
                    CTDONHANG ctdh = db.CTDONHANGs.Where(x => x.DonHang_ID == item.DonHang_ID && x.Mon_ID == item.Mon_ID).SingleOrDefault();
                    db.CTDONHANGs.Remove(ctdh);
                }
                DONHANG dh = db.DONHANGs.Find(id);
                db.DONHANGs.Remove(dh);
                db.SaveChanges();
                ViewBag.Mess = mess;
            }
            catch
            {
                return View("Error");
            }
            return View();
        }
        public void BuildEmailTemplate(string MaDH, string MaKH, double TienDonHang)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/Assets/template/") + "Text" + ".cshtml");
            var dh = db.DONHANGs.Where(x => x.DonHang_ID == MaDH).FirstOrDefault();
            var kh = db.KHACHHANGs.Where(x => x.KhachHang_ID == MaKH).FirstOrDefault();
            /*var url = "http://fastfoodnumone.somee.com/" + "GioHang/XacNhanTrangThai?maDH=" + MaDH;*/
            var url = "http://localhost:57687/" + "GioHang/XacNhanTrangThai?maDH=" + MaDH;
            body = body.Replace("@ViewBag.LinkXacNhan", url);
            body = body.Replace("@ViewBag.HoTenKH", kh.HoTen);
            body = body.Replace("@ViewBag.SDT", kh.SDTKH);
            body = body.Replace("@ViewBag.DiaChi", kh.DiaChi);
            body = body.Replace("@ViewBag.MaDH", dh.DonHang_ID);
            body = body.Replace("@ViewBag.TongTien", TienDonHang.ToString());
            body = body.ToString();
            GuiEmail("Đặt hàng thành công", body, kh.Email);

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
            if(!string.IsNullOrEmpty(bcc))
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
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult XacNhanTrangThai(string maDH)
        {
            DONHANG dh = db.DONHANGs.Find(maDH);
            if (dh.DaXacNhan == false)
            {
                dh.DaXacNhan = true;
                db.Entry(dh).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("ConfirmDH");
        }

        public ActionResult ConfirmDH(string MaDH)
        {
            return View();
        }

        public ActionResult XacNhanDonHang()
        {
            if(Session["KhachHang"] != null && Session["DonHang"] != null)
            {
                DONHANG dh = (DONHANG)Session["DonHang"];
                ViewBag.TongSoLuong = db.CTDONHANGs.Where(x => x.DonHang_ID == dh.DonHang_ID).Sum(x => x.SoLuongMua);
                ViewBag.TongTien = db.CTDONHANGs.Where(x => x.DonHang_ID == dh.DonHang_ID).Sum(x => x.ThanhTien);
                ViewBag.TongKM = TongTienKM(dh.Voucher_ID, dh.DonHang_ID);
                return View(db.CTDONHANGs.Where(x => x.DonHang_ID == dh.DonHang_ID).ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }
        public IList<GioHangExcel> LayGioHangExcel()
        {
            KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
            DONHANG dh = (DONHANG)Session["DonHang"];
            var ExcelList = (from a in db.CTDONHANGs
                             join b in db.DONHANGs
                              on a.DonHang_ID equals b.DonHang_ID
                             join c in db.MONs on a.Mon_ID equals c.Mon_ID
                             where b.KhachHang_ID == kh.KhachHang_ID && b.DonHang_ID == dh.DonHang_ID
                             select new GioHangExcel
                             {
                                 MaDonHang = dh.DonHang_ID,
                                 DiaChiNhan = kh.DiaChi,
                                 NgayDat = (DateTime)b.ThoiGianDat,
                                 TenMon = c.TenMon,
                                 DonGia = (double)a.DonGiaMua,
                                 SoLuong = (int)a.SoLuongMua             
                             }).ToList();
            return ExcelList;
        }

        public ActionResult XuatExcel()
        {
            var gv = new GridView();
            gv.DataSource = this.LayGioHangExcel();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DonHang.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View();
        }
    }
}