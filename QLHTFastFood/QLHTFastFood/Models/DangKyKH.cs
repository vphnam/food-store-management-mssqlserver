using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLHTFastFood.Models
{
    public class DangKyKH
    {
        [DisplayName("Họ tên")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string HoTen { get; set; }
        [DisplayName("Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string SDTKH { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string Email { get; set; }
        [DisplayName("Tài khoản")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string UserName { get; set; }
        [DisplayName("Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string PassWord { get; set; }
        [DisplayName("Xác nhận mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string ConfirmPassWord { get; set; }
        [DisplayName("Địa chỉ")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string DiaChi { get; set; }
    }
}