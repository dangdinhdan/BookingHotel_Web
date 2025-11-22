using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelBooking_Web.Models
{
    public class EditProfileViewModel
    {
        [Display(Name = "Tên người dùng")]
        // Thuộc tính [Required] đảm bảo người dùng phải nhập thông tin này
        [Required(ErrorMessage = "Tên người dùng không được để trống.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên người dùng phải dài từ 3 đến 100 ký tự.")]
        public string TenNguoiDung { get; set; }

        [Display(Name = "Địa chỉ Email")]
        [Required(ErrorMessage = "Email không được để trống.")]
        // Thuộc tính [EmailAddress] kiểm tra định dạng email chuẩn
        [EmailAddress(ErrorMessage = "Địa chỉ Email không hợp lệ.")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        // [RegularExpression] dùng để kiểm tra số điện thoại có đúng định dạng không
        [RegularExpression(@"^\+?\d{9,15}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string SoDienThoai { get; set; }
    }
}