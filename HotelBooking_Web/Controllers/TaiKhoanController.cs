using HotelBooking_Web.Models;
using HotelBooking_Web.Services; // <-- Bổ sung using cho CustomerService
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelBooking_Web.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly CustomerService _service = new CustomerService();

        private EditProfileViewModel GetMockUserProfile()
        {
            return new EditProfileViewModel
            {
                TenNguoiDung = Session["DisplayName"]?.ToString() ?? "Khách Hàng",
                Email = Session["UserEmail"]?.ToString() ?? "khach@gmail.com",
                SoDienThoai = "0901234567" // Dữ liệu giả
            };
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(TaiKhoanModel model)
        {
            var existingUser = _service.GetAccountIncludeDeleted(model.Email);

            if (existingUser != null)
            {
                if (existingUser.isDelete == false)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại!");
                }
                else
                {
                    // Bỏ qua lỗi Validation của Email (nếu có) vì ta sẽ dùng lại email này
                    // Logic khôi phục nằm bên dưới
                }
            }

            if (ModelState.IsValid)
            {
                if (existingUser != null && existingUser.isDelete == true)
                {
                    _service.RestoreCustomer(model, model.MatKhau);
                    ViewBag.Message = "Chào mừng trở lại! Tài khoản cũ của bạn đã được khôi phục thành công.";
                }
                else
                {
                    _service.RegisterCustomer(model, model.MatKhau);
                    ViewBag.Message = "Đăng ký thành công!";
                }

                return View("RegisterSuccess");
            }

            return View(model);
        }

        //public ActionResult ConfirmEmail(string token)
        //{
        //    if (_service.ConfirmEmail(token))
        //    {
        //        ViewBag.Message = "Xác nhận email thành công!";
        //    }
        //    else
        //    {
        //        ViewBag.Message = "Link xác nhận không hợp lệ hoặc đã hết hạn.";
        //    }
        //    return View();
        //}

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            string hashedInputPassword = _service.HashPassword(password);

            using (var db = new HotelDbContext())
            {
                var user = db.TaiKhoans.FirstOrDefault(u => u.Email == email && u.MatKhau == hashedInputPassword && u.isDelete == false);

                if (user != null)
                {
                    // THÀNH CÔNG
                    Session["UserEmail"] = user.Email;
                    Session["DisplayName"] = user.HoTen;
                    Session["IsAdmin"] = (user.VaiTro == "Admin");

                    return RedirectToAction("Index", "Home");
                }
            }

            // THẤT BẠI
            ViewBag.Error = "Email hoặc mật khẩu không đúng. Vui lòng thử lại.";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Profile()
        {
            if (Session["UserEmail"] == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.UserPhone = "0901234567";
            ViewBag.MemberSince = "Tháng 11, 2024";

            return View();
        }

        public ActionResult EditProfile()
        {
            if (Session["UserEmail"] == null) return RedirectToAction("Login");

            string email = Session["UserEmail"].ToString();

            var user = _service.GetCustomerByEmail(email);

            if (user == null) return RedirectToAction("Login");

            var model = new EditProfileViewModel
            {
                TenNguoiDung = user.HoTen,
                Email = user.Email,
                SoDienThoai = user.SoDienThoai,
                Address = user.DiaChi
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(EditProfileViewModel model)
        {
            if (Session["UserEmail"] == null) return RedirectToAction("Login");

            string email = Session["UserEmail"].ToString();

            model.Email = email;

            if (ModelState.IsValid)
            {
                try
                {
                    _service.UpdateProfile(email, model);

                    Session["DisplayName"] = model.TenNguoiDung;

                    TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";

                    return RedirectToAction("EditProfile");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    // Đoạn này giúp lấy chi tiết lỗi từ bên trong Entity Framework ra ngoài
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Nối các lỗi lại thành 1 chuỗi để hiển thị
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Hiển thị ra TempData
                    TempData["ErrorMessage"] = "Lỗi dữ liệu: " + fullErrorMessage;
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Vui lòng kiểm tra lại thông tin nhập vào.";
            }

            return View(model);
        }

        public ActionResult ChangePassword()
        {
            if (Session["UserEmail"] == null) return RedirectToAction("Login");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            if (Session["UserEmail"] == null) return RedirectToAction("Login");

            if (string.IsNullOrEmpty(OldPassword))
            {
                ModelState.AddModelError("OldPassword", "Vui lòng nhập mật khẩu hiện tại.");
            }
            if (NewPassword.Length < 6)
            {
                ModelState.AddModelError("NewPassword", "Mật khẩu mới phải từ 6 ký tự trở lên.");
            }
            if (NewPassword != ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Mật khẩu xác nhận không khớp.");
            }

            if (ModelState.IsValid)
            {
                string email = Session["UserEmail"].ToString();
                string error = "";

                bool result = _service.ChangePassword(email, OldPassword, NewPassword, out error);

                if (result)
                {
                    TempData["Success"] = "Mật khẩu đã được thay đổi thành công!";
                    return RedirectToAction("ChangePassword");
                }
                else
                {
                    ModelState.AddModelError("OldPassword", error);
                }
            }
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
                TempData["DeleteError"] = "Bạn phải tick vào ô xác nhận trước khi xóa tài khoản.";
                return RedirectToAction("DeleteAccount");
            }

            try
            {
                string email = Session["UserEmail"].ToString();

                _service.DeleteAccount(email);

                Session.Clear();
                Session.Abandon();

                TempData["DeleteSuccess"] = "Tài khoản của bạn đã được xóa thành công. Hẹn gặp lại!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["DeleteError"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("DeleteAccount");
            }
        }


        public ActionResult BookingList()
        {
            if (Session["UserEmail"] == null) return RedirectToAction("Login");
            return View();
        }

        public ActionResult TransactionHistory()
        {
            if (Session["UserEmail"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public ActionResult Favourite()
        {
            if (Session["UserEmail"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
    }
}