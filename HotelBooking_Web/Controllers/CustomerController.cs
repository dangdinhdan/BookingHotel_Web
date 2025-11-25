using System.Web.Mvc;
using HotelBooking_Web.Models;
using HotelBooking_Web.Services;

namespace HotelBooking_Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerService _service = new CustomerService();

        // GET: Customer/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Customer/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Customer model, string password, string confirmPassword)
        {
            if (_service.IsEmailExist(model.Email))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại!");
            }

            if (password != confirmPassword)
            {
                ModelState.AddModelError("Password", "Mật khẩu xác nhận không khớp!");
            }

            if (ModelState.IsValid)
            {
                _service.RegisterCustomer(model, password);
                ViewBag.Message = "Đăng ký thành công! Vui lòng kiểm tra email để xác nhận.";
                return View("RegisterSuccess");
            }

            return View(model);
        }

        // GET: Customer/ConfirmEmail
        public ActionResult ConfirmEmail(string token)
        {
            if (_service.ConfirmEmail(token))
            {
                ViewBag.Message = "Xác nhận email thành công!";
            }
            else
            {
                ViewBag.Message = "Link xác nhận không hợp lệ hoặc đã hết hạn.";
            }
            return View();
        }
    }
}
