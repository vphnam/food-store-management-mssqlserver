using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLHTFastFood.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Password")]
        public string PassWord { get; set; }
        public bool RememberMe { get; set; }
    }
}