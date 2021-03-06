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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class CTDONHANG
    {
        [DisplayName("Mã đơn hàng")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string DonHang_ID { get; set; }
        [DisplayName("Mã món")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string Mon_ID { get; set; }
        [DisplayName("Đơn giá mua")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public Nullable<decimal> DonGiaMua { get; set; }
        [DisplayName("Số lượng mua")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public Nullable<int> SoLuongMua { get; set; }
        [DisplayName("Thành tiền")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public Nullable<decimal> ThanhTien { get; set; }
    
        public virtual DONHANG DONHANG { get; set; }
        public virtual MON MON { get; set; }
    }
}
