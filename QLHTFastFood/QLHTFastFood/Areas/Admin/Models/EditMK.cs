using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLHTFastFood.Areas.Admin.Models
{
    public class EditMK
    {
        [DisplayName("Mật khẩu mới")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string NewPassWord { get; set; }
        [DisplayName("Xác nhận mật khẩu")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string ConfirmNewPassword { get; set; }
    }
}