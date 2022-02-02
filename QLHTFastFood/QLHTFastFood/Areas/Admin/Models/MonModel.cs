using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLHTFastFood.Areas.Admin.Models
{
    public class MonModel
    {
        public string MaMon { get; set; }
        public string TenMon { get; set; }
        [DisplayName("Đơn giá")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Đơn giá phải là số")]
        public int DonGia { get; set; }
        public string KhuyenMai_ID { get; set; }
    }
}