using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLHTFastFood.Areas.Admin.Models
{
    public class DonHangExcel
    {
        public string TenMon { get; set; }
        public Nullable<decimal> DonGiaMua { get; set; }
        public Nullable<int> SoLuongMua { get; set; }
        public Nullable<decimal> ThanhTien { get; set; }
    }
}