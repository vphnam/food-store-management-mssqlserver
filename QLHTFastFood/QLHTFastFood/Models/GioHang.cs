using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLHTFastFood.Models
{
    public class GioHang
    {
        QLFastFoodEntities db = new QLFastFoodEntities();
        public string maMon { get; set; }
        public string tenMon { get; set; }
        public string anhMon { get; set; }
        public Double donGia { get; set; }
        public int soLuong { get; set; }
        public Double thanhTien
        {
            get { return soLuong * donGia; }
        }
        public GioHang(string id, int sl)
        {
            maMon = id;
            MON mon = db.MONs.Find(id);   
            tenMon = mon.TenMon;
            anhMon = mon.AnhMon;
            if(mon.KhuyenMai_ID != null)
            {
                KHUYENMAI km = db.KHUYENMAIs.Find(mon.KhuyenMai_ID);
                donGia = (double)mon.DonGia - (double)mon.DonGia * (double)km.PhanTramKhuyenMai;

            }
            else
            {
                donGia = double.Parse(mon.DonGia.ToString());
            }
            soLuong = sl;
        }
    }
}