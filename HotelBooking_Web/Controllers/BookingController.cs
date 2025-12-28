using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelBooking_Web.Models; // Quan trọng: Để nhận diện được ViewModel và DB Context

namespace HotelBooking_Web.Controllers
{
    public class BookingController : Controller
    {
        // 1. Khởi tạo kết nối CSDL
        // Lưu ý: Đổi 'DataClasses1DataContext' thành tên đúng trong file .dbml của bạn nếu nó khác
        private DataClasses1DataContext db = new DataClasses1DataContext();

        // GET: /Booking/Bookings
        // Trang hiển thị thông tin xác nhận
        [Authorize] // Bắt buộc đăng nhập mới vào được
        public ActionResult Bookings(int? phongId, DateTime? checkin, DateTime? checkout, int? guests)
        {
            // Kiểm tra tham số đầu vào
            if (phongId == null || checkin == null || checkout == null)
            {
                return RedirectToAction("Index", "Home"); // Hoặc trang báo lỗi
            }

            // Lấy ID người dùng từ Session (đã đăng nhập)
            if (Session["TaiKhoan"] == null && Session["TaiKhoanID"] == null)
            {
                // Nếu mất session, đá về login
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Url.AbsoluteUri });
            }

            // Lấy TaiKhoanID an toàn
            int userId = 0;
            if (Session["TaiKhoanID"] != null)
            {
                userId = (int)Session["TaiKhoanID"];
            }
            else
            {
                // Logic dự phòng nếu bạn lưu cả object User trong Session
                var userSession = (tbl_TaiKhoan)Session["TaiKhoan"];
                userId = userSession.TaiKhoanID;
            }

            // Tìm thông tin User và Phòng trong DB
            var user = db.tbl_TaiKhoans.FirstOrDefault(u => u.TaiKhoanID == userId);
            var room = db.tbl_Phongs.FirstOrDefault(p => p.PhongID == phongId);

            if (user == null || room == null) return HttpNotFound();

            // Tính số ngày ở
            int soNgay = (int)(checkout.Value - checkin.Value).TotalDays;
            if (soNgay <= 0) soNgay = 1;

            // Đổ dữ liệu vào ViewModel
            BookingsModel model = new BookingsModel
            {
                PhongID = room.PhongID,
                TenPhong = room.SoPhong, // Hoặc room.TenLoaiPhong nếu muốn
                HinhAnh = room.HinhAnh,
                GiaMoiDem = (decimal)room.GiaMoiDem,

                NgayNhanPhong = checkin.Value,
                NgayTraPhong = checkout.Value,
                SoLuongNguoi = guests ?? 1, // Nếu null thì mặc định 1
                SoNgayO = soNgay,
                TongTien = (decimal)room.GiaMoiDem * soNgay,

                // Thông tin khách
                TaiKhoanID = user.TaiKhoanID,
                HoTenKH = user.HoTen,
                SDTKH = user.SoDienThoai,
                EmailKH = user.Email
            };

            return View(model); // Trả về View "Bookings.cshtml"
        }

        // POST: /Booking/Bookings
        // Xử lý lưu đơn đặt phòng
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Bookings(BookingsModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Map dữ liệu sang bảng tbl_DatPhong
                    tbl_DatPhong booking = new tbl_DatPhong();

                    booking.TaiKhoanID = model.TaiKhoanID;
                    booking.PhongID = model.PhongID;
                    booking.NgayDat = DateTime.Now; // Ngày hiện tại
                    booking.NgayNhanPhong = model.NgayNhanPhong;
                    booking.NgayTraPhong = model.NgayTraPhong;
                    booking.SoLuongNguoi = model.SoLuongNguoi;
                    booking.TongTien = model.TongTien;
                    booking.GhiChu = model.GhiChu;

                    // Các cột trạng thái
                    booking.TrangThai = "ChoXacNhan";
                    booking.isDelete = false;

                    // Lưu vào DB
                    db.tbl_DatPhongs.InsertOnSubmit(booking);
                    db.SubmitChanges();

                    // Thành công -> Chuyển sang trang thông báo
                    return RedirectToAction("BookingSuccess");
                }
                catch (Exception ex)
                {
                    // Ghi lỗi để hiển thị ra View
                    ModelState.AddModelError("", "Lỗi hệ thống: " + ex.Message);
                }
            }

            // Nếu lỗi (Model không hợp lệ hoặc lỗi Try/Catch) thì hiện lại Form cũ
            return View(model);
        }

        // Trang thông báo thành công
        public ActionResult BookingSuccess()
        {
            return View();
        }
    }
}