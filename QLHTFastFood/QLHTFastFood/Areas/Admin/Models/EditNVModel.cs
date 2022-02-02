using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLHTFastFood.Areas.Admin.Models
{
    public class EditNVModel
    {
        [DisplayName("Họ tên nhân viên")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string HoTenNV { get; set; }

        [DisplayName("Quyền truy cập")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string Quyen_ID { get; set; }
        [DisplayName("Ngày sinh")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public Nullable<System.DateTime> NgaySinh { get; set; }
        [DisplayName("Địa chỉ")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string DiaChi { get; set; }
        [DisplayName("Số điện thoại")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string SDT { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string Email { get; set; }
    }
}