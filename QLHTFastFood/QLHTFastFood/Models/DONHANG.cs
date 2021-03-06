//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLHTFastFood.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DONHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DONHANG()
        {
            this.CTDOANHTHUNGAYs = new HashSet<CTDOANHTHUNGAY>();
            this.CTDONHANGs = new HashSet<CTDONHANG>();
        }
    
        public string DonHang_ID { get; set; }
        public string KhachHang_ID { get; set; }
        public string NhanVien_ID { get; set; }
        public Nullable<bool> Loai { get; set; }
        public Nullable<bool> DaXacNhan { get; set; }
        public Nullable<bool> DaThanhToan { get; set; }
        public Nullable<System.DateTime> ThoiGianDat { get; set; }
        public Nullable<System.DateTime> ThoiGianGiao { get; set; }
        public string Voucher_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTDOANHTHUNGAY> CTDOANHTHUNGAYs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTDONHANG> CTDONHANGs { get; set; }
        public virtual KHACHHANG KHACHHANG { get; set; }
        public virtual NHANVIEN NHANVIEN { get; set; }
        public virtual VOUCHER VOUCHER { get; set; }
    }
}
