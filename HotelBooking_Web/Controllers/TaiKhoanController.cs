using HotelBooking_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelBooking_Web.Controllers
{
    public class TaiKhoanController : Controller
    {
        // Hàm tạo dữ liệu giả lập cho trang Profile
        private EditProfileViewModel GetMockUserProfile()
        {
            return new EditProfileViewModel
            {
                // Lấy tên hiển thị từ Session, nếu chưa có thì hiện mặc định
                TenNguoiDung = Session["DisplayName"]?.ToString() ?? "Khách Hàng",
                // Lấy Email từ Session (đây là định danh chính)
                Email = Session["UserEmail"]?.ToString() ?? "khach@gmail.com",
                SoDienThoai = "0901234567" // Dữ liệu giả
            };
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            // --- TÀI KHOẢN TEST CỐ ĐỊNH ---
            string testEmail = "khach@gmail.com";
            string testPass = "123456";

            // Kiểm tra: Nếu đúng email test HOẶC admin (để bạn dễ test)
            if ((email == testEmail && password == testPass) || (email == "admin@gmail.com" && password == "123456"))
            {
                // 1. Lưu Email làm định danh chính (Key: UserEmail)
                Session["UserEmail"] = email;
                
                // 2. Tạo tên hiển thị giả (Lấy phần trước @ làm tên)
                string displayName = email.Split('@')[0];
                Session["DisplayName"] = displayName;

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Sai email hoặc mật khẩu! (Thử: khach@gmail.com / 123456)";
            return View();
        }

        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string username, string email, string password)
        {
            // GIẢ LẬP: Đăng ký xong tự động đăng nhập luôn
            Session["UserEmail"] = email;
            Session["DisplayName"] = username; // Username lúc đăng ký đóng vai trò là Họ Tên

            TempData["msg"] = "Đăng ký thành công! Đã tự động đăng nhập.";
            
            // Về trang chủ luôn cho tiện (giống Booking.com)
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Clear(); // Xóa hết UserEmail, DisplayName...
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Profile()
        {
            // Kiểm tra đăng nhập bằng key UserEmail
            if (Session["UserEmail"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public ActionResult EditProfile()
        {
            if (Session["UserEmail"] == null) return RedirectToAction("Login");
            // Truyền model vào view để hiện thông tin cũ
            var model = GetMockUserProfile();
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(EditProfileViewModel model)
        {
             // Giả lập lưu: Cập nhật lại Session tên hiển thị
             if(ModelState.IsValid)
             {
                 Session["DisplayName"] = model.TenNguoiDung;
                 TempData["Success"] = "Cập nhật hồ sơ thành công!";
             }
             return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult DeleteAccount()
        {
            if (Session["UserEmail"] == null)
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAccount(string reason, bool? confirm)
        {
            if (Session["UserEmail"] == null)
                return RedirectToAction("Login");

            if (confirm != true)
            {
                TempData["DeleteError"] = "Bạn phải xác nhận trước khi xóa tài khoản.";
                return RedirectToAction("DeleteAccount");
            }

            string deletedUser = Session["UserEmail"].ToString();
            Session.Clear(); // Logout

            TempData["DeleteSuccess"] = $"Tài khoản '{deletedUser}' đã được xóa thành công.";
            return RedirectToAction("Index", "Home");
        }
    }
}