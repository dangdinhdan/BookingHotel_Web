using HotelBooking_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelBooking_Web.Controllers
{
    public class TaiKhoanController : Controller
    {
        private EditProfileViewModel GetMockUserProfile()
        {
            return new EditProfileViewModel
            {
                // Lấy tên từ Session để đảm bảo dữ liệu khớp với người dùng đang đăng nhập
                TenNguoiDung = Session["username"]?.ToString() ?? "Người dùng",
                Email = "user.email@hotel.com",
                SoDienThoai = "0901234567" // Dữ liệu giả
            };
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            // Tạm thời cho phép đăng nhập demo
            if (username == "khongchoisaobietminhkhongthe" && password == "1236")
            {
                Session["username"] = username;
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu!";
            return View();
        }

        // GET: Register
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(string username, string password)
        {
            // Chỉ demo: không lưu DB
            TempData["msg"] = "Đăng ký thành công! Mời đăng nhập.";
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Profile()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public ActionResult EditProfile()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult DeleteAccount()
        {
            // Kiểm tra nếu chưa đăng nhập → đá về login
            if (Session["username"] == null)
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAccount(string reason, bool? confirm)
        {
            if (Session["username"] == null)
                return RedirectToAction("Login");

            // Chưa tick xác nhận
            if (confirm != true)
            {
                TempData["DeleteError"] = "Bạn phải xác nhận trước khi xóa tài khoản.";
                return RedirectToAction("DeleteAccount");
            }

            // Nếu sau này có DB → xóa tại đây
            // Example:
            // _db.Users.Remove(user);
            // _db.SaveChanges();

            string deletedUser = Session["username"].ToString();

            // Xóa session → log out
            Session.Clear();

            TempData["DeleteSuccess"] = $"Tài khoản '{deletedUser}' đã được xóa thành công.";

            return RedirectToAction("Index", "Home");
        }
    }
    
}
