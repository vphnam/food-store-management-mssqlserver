using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLHTFastFood.Models
{
    public class DetailKH
    {
        [DisplayName("Mã khách hàng")]
        public string KhachHang_ID { get; set; }
        [DisplayName("Họ tên")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string HoTen { get; set; }
        [DisplayName("Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string SDTKH { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string Email { get; set; }
        [DisplayName("Địa chỉ")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string DiaChi { get; set; }
    }
}