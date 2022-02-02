using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLHTFastFood.Models
{
    public partial class DisplayMon
    {
        public DisplayMon()
        { }
        
        public string Mon_ID { get; set; }
        public string TenMon { get; set; }
        public string Loai_ID { get; set; }
        public string Size_ID { get; set; }
        public string DonVi_ID { get; set; }
        public string AnhMon { get; set; }
        public Nullable<decimal> DonGia { get; set; }
        public string KhuyenMai_ID { get; set; }
        public double GiaKM { get; set; }
        public string MoTa { get; set; }
        public virtual LOAI LOAI { get; set; }
        public virtual SIZE SIZE { get; set; }
        public virtual DONVI DONVI { get; set; }
        public virtual KHUYENMAI KHUYENMAI { get; set; }
    }
}