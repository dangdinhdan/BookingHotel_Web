using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelBooking_Web.Models
{
    public class EditProfileViewModel
    {
        [Display(Name = "Họ và tên")] 
        [Required(ErrorMessage = "Họ tên không được để trống.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên phải dài từ 3 đến 100 ký tự.")]
        public string TenNguoiDung { get; set; }

        [Display(Name = "Địa chỉ Email")]
        [Required]
        [EmailAddress(ErrorMessage = "Địa chỉ Email không hợp lệ.")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        [RegularExpression(@"^\+?\d{9,15}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string SoDienThoai { get; set; }
    }
}