using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLHTFastFood.Areas.Admin.Models
{
    public class CreateKHModel
    {
        [DisplayName("Họ tên khách hàng")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string HoTen { get; set; }
        [DisplayName("Số điện thoại khách hàng")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string SDTKH { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string Email { get; set; }
        [DisplayName("Tài khoản")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string UserName { get; set; }
        [DisplayName("Mật khẩu")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string PassWord { get; set; }
        [DisplayName("Xác nhận mật khẩu")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string ConfirmPassword { get; set; }
        [DisplayName("Địa chỉ")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string DiaChi { get; set; }
    }
}