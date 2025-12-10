using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking_Web.Models
{
    [Table("tbl_TaiKhoan")]
    public class TaiKhoanModel
    {
        [Key]
        public int TaiKhoanID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string MaTK { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
        [StringLength(100)]
        public string HoTen { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Vui lòng nhập Email.")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
        [StringLength(100)]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải từ 6 ký tự trở lên.")]
        public string MatKhau { get; set; }

        [NotMapped] 
        [Display(Name = "Xác nhận Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu.")]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu nhập lại không khớp!")] 
        public string ConfirmPassword { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [StringLength(10, ErrorMessage = "Số điện thoại tối đa 10 số.")]
        [RegularExpression(@"^\d{9,10}$", ErrorMessage = "SĐT phải là 9-10 chữ số.")]
        public string SoDienThoai { get; set; }

        public string DiaChi { get; set; }

        public DateTime Create_at { get; set; } = DateTime.Now;

        public string VaiTro { get; set; } = "customer";

        public DateTime? Update_at { get; set; }
        public bool isDelete { get; set; } = false;
        public DateTime? Delete_at { get; set; }
    }
}