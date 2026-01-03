//using System.Web.Mvc;
//using HotelBooking_Web.Models;
//using HotelBooking_Web.Services;

//namespace HotelBooking_Web.Controllers
//{
//    public class CustomerController : Controller
//    {
//        private readonly CustomerService _service = new CustomerService();

//        // GET: Customer/Register
//        public ActionResult Register()
//        {
//            return View();
//        }

//        // POST: Customer/Register
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        // [FIX LỖI L22]: BỎ THAM SỐ "string password" vì nó bị trùng với model.MatKhau
//        public ActionResult Register(TaiKhoanModel model, string confirmPassword)
//        {
//            if (_service.IsEmailExist(model.Email))
//            {
//                ModelState.AddModelError("Email", "Email đã tồn tại!");
//            }

//            // [FIX LỖI]: Dùng model.MatKhau (Mật khẩu thô từ form) để so sánh
//            if (model.MatKhau != confirmPassword)
//            {
//                ModelState.AddModelError("MatKhau", "Mật khẩu xác nhận không khớp!");
//            }

//            if (ModelState.IsValid)
//            {
//                // GỌI SERVICE: Truyền model và mật khẩu thô (model.MatKhau)
//                // Service sẽ tự HASH và lưu vào DB
//                _service.RegisterCustomer(model, model.MatKhau);

//                ViewBag.Message = "Đăng ký thành công!";
//                return View("RegisterSuccess");
//            }

//            return View(model);
//        }

//        // GET: Customer/ConfirmEmail
//        //public ActionResult ConfirmEmail(string token)
//        //{
//        //    if (_service.ConfirmEmail(token))
//        //    {
//        //        ViewBag.Message = "Xác nhận email thành công!";
//        //    }
//        //    else
//        //    {
//        //        ViewBag.Message = "Link xác nhận không hợp lệ hoặc đã hết hạn.";
//        //    }
//        //    return View();
//        //}
//    }
//}
