using System.ComponentModel.DataAnnotations;

namespace HotelBooking_Web.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải từ 6 ký tự trở lên")]
        public string PasswordHash { get; set; }

        public bool IsEmailConfirmed { get; set; } = false;
        public string EmailConfirmationToken { get; set; }
    }
}
