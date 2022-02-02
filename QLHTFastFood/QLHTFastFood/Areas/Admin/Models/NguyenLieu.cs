using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLHTFastFood.Areas.Admin.Models
{
    public class NguyenLieu
    {
        [DisplayName("Số lượng nguyên liệu cần")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Vui lòng chỉ nhập số hoặc số thập phân")]
        public Nullable<decimal> SoLuongNLCan { get; set; }
    }
}