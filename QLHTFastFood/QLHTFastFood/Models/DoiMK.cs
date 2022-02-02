using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLHTFastFood.Models
{
    public class DoiMK
    {
        [DisplayName("Mật khẩu cũ")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string PassWordCu { get; set; }
        [DisplayName("Mật khẩu mới")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string PassWord { get; set; }
        [DisplayName("Xác nhận mật khẩu mới")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string ConfirmPassWord { get; set; }
    }
}