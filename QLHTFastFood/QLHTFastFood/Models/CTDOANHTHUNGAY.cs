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
    
    public partial class CTDOANHTHUNGAY
    {
        public System.DateTime NgayGio { get; set; }
        public string DonHang_ID { get; set; }
        public Nullable<bool> Loai { get; set; }
        public Nullable<decimal> TongTienHoaDon { get; set; }
    
        public virtual DONHANG DONHANG { get; set; }
    }
}
