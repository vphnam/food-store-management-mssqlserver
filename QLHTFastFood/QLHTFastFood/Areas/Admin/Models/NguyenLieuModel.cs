using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLHTFastFood.Areas.Admin.Models
{
    public class NguyenLieuModel
    {
        public string MaNguyenLieu { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số lượng tồn")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Vui lòng chỉ nhập số hoặc số thập phân")]
        public double SoLuongTon { get; set; }
    }
}