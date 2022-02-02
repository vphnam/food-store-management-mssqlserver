using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLHTFastFood.Areas.Admin.Models
{
    public class MonEn
    {
        public MonEn()
        {
            AnhMon = "~/Images/add.png";
        }
        public string Mon_ID { get; set; }
        [DisplayName("Tên món")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string TenMon { get; set; }
        [DisplayName("Loại món")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string Loai_ID { get; set; }
        [DisplayName("Size món")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string Size_ID { get; set; }
        [DisplayName("Đơn vị")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string DonVi_ID { get; set; }
        [DisplayName("Ảnh minh họa")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string AnhMon { get; set; }
        [DisplayName("Đơn giá")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Đơn giá phải là số")]
        public Nullable<decimal> DonGia { get; set; }
        public string KhuyenMai_ID { get; set; }
        [DisplayName("Mô tả")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public string MoTa { get; set; }

        [DisplayName("Ảnh minh họa")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public HttpPostedFileBase UploadImage { get; set; }
    }
}