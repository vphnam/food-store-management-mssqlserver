using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace QLHTFastFood.Models
{
    public class GioHangExcel
    {
        [DisplayName("Mã đơn hàng")]
        public string MaDonHang { get; set; }
        [DisplayName("Địa chỉ")]
        public string DiaChiNhan { get; set; }
        [DisplayName("Têm món")]
        public string TenMon { get; set; }
        [DisplayName("Đơn giá")]
        public double DonGia { get; set; }
        [DisplayName("Số lượng")]
        public int SoLuong { get; set; }
        [DisplayName("Thành tiền")]
        public Double Thanhtien
        {
            get { return SoLuong * DonGia; }
        }
        [DisplayName("Ngày đặt")]
        public DateTime NgayDat { get; set; }
    }
}