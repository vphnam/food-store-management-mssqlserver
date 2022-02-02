using OfficeOpenXml;
using PagedList;
using QLHTFastFood.Areas.Admin.Models;
using QLHTFastFood.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLHTFastFood.Areas.Admin.Controllers
{
    public class DonHangController : BaseController
    {
        // GET: Admin/DonHang
        QLFastFoodEntities db = new QLFastFoodEntities();
        public ActionResult Index(string id, int page = 1, int pageSize = 50)
        {
            if (id != null && id != "")
            {
                return View(db.DONHANGs.Where(x => x.DonHang_ID.StartsWith(id)).OrderBy(x => x.DonHang_ID).ToPagedList(page, pageSize));
            }
            else
            {
                return View(db.DONHANGs.OrderByDescending(x => x.DonHang_ID).ToPagedList(page, pageSize));
            }
        }
        public ActionResult ChiTietDH(string id, int page = 1, int pageSize = 50)
        {
            ViewBag.MaDH = id;
            return View(db.CTDONHANGs.Where(x => x.DonHang_ID == id).OrderBy(x => x.Mon_ID).ToPagedList(page, pageSize));
        }
        public ActionResult ThanhToan(string maDH)
        {
            DONHANG dh = db.DONHANGs.Find(maDH);
            NHANVIEN nv = Session["NhanVienAdmin"] as NHANVIEN;
            if (dh != null)
            {
                dh.NhanVien_ID = nv.NhanVien_ID;
                dh.DaThanhToan = true;
                db.Entry(dh).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                SetAlert("Thanh toán đơn hàng thành công", "success");
                return RedirectToAction("Index");
            }
            else
            {
                SetAlert("Thanh toán đơn hàng thất bại", "error");
                return RedirectToAction("Index");
            }
        }
        public ActionResult XacNhan(string maDH)
        {
            try
            {
                DONHANG dh = db.DONHANGs.Find(maDH);
                if (dh != null)
                {
                    dh.DaXacNhan = true;
                    db.Entry(dh).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    SetAlert("Thanh toán đơn hàng thành công", "success");
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Thanh toán đơn hàng thất bại", "error");
                    return RedirectToAction("Index");
                }
            }

            catch
            {
                SetAlert("Thanh toán đơn hàng thất bại", "error");
                return RedirectToAction("Index");
            }
        }
        public List<DonHangExcel> LayCTDHExcel(string id)
        {
            List<CTDONHANG> lstctdh = new List<CTDONHANG>();
            lstctdh = db.CTDONHANGs.Where(x => x.DonHang_ID == id).OrderBy(x => x.Mon_ID).ToList();
            List<DonHangExcel> lstdh = new List<DonHangExcel>();
            foreach (var item in lstctdh)
            {
                DonHangExcel dhex = new DonHangExcel();
                dhex.TenMon = item.MON.TenMon;
                dhex.DonGiaMua = item.DonGiaMua;
                dhex.SoLuongMua = item.SoLuongMua;
                dhex.ThanhTien = item.ThanhTien;
                lstdh.Add(dhex);
            }
            return lstdh.ToList();
        }
        public ActionResult XuatExcel(string id)
        {
            DONHANG dh = db.DONHANGs.Find(id);
            List<DonHangExcel> lstCTDH = LayCTDHExcel(id);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("HoaDon");

            ws.Cells["A1"].Value = "MÃ ĐƠN HÀNG";
            ws.Cells["B1"].Value = id;

            ws.Cells["A2"].Value = "TÊN KHÁCH HÀNG";
            ws.Cells["B2"].Value = dh.KHACHHANG.HoTen.ToString();

            ws.Cells["C2"].Value = "TÊN NHÂN VIÊN";
            if(dh.NhanVien_ID != null)
            {
                ws.Cells["D2"].Value = dh.NHANVIEN.HoTenNV.ToString();
            }
            else
            {
                ws.Cells["D2"].Value = "";
            }      

            DateTime timeDat = (DateTime)dh.ThoiGianDat;
            ws.Cells["A3"].Value = "THỜI GIAN ĐẶT";
            ws.Cells["B3"].Value = string.Format("{0:dd/MM/yyyy} vào lúc {0:H: mm tt}", timeDat);

            DateTime timeGiao = (DateTime)dh.ThoiGianGiao;
            ws.Cells["C3"].Value = "THỜI GIAN GIAO";
            ws.Cells["D3"].Value = string.Format("{0:dd/MM/yyyy} vào lúc {0:H: mm tt}", timeGiao);

            ws.Cells["A6"].Value = "TÊN MÓN";
            ws.Cells["B6"].Value = "ĐƠN GIÁ MUA";
            ws.Cells["C6"].Value = "SỐ LƯỢNG MUA";
            ws.Cells["D6"].Value = "THÀNH TIỀN";

            int rowStart = 7;
            foreach(var item in lstCTDH)
            {
                ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("white")));
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.TenMon.ToString();
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.DonGiaMua;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.SoLuongMua;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.ThanhTien;
                rowStart++;
            }
            ws.Cells[string.Format("C{0}", rowStart)].Value = "TỔNG TIỀN";
            ws.Cells[string.Format("D{0}", rowStart)].Value = db.CTDONHANGs.Where(x => x.DonHang_ID == id).Sum(x => x.ThanhTien);
            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetxml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=HoaDon" + dh.DonHang_ID +".xls");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
            return View();
        }

    }
}