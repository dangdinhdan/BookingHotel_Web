using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBooking_Web.Models
{
    public class BookingsModel
    {
        // Thông tin Phòng
        public int PhongID { get; set; }
        public string TenPhong { get; set; }
        public string HinhAnh { get; set; }
        public decimal GiaMoiDem { get; set; }

        // Thông tin Đặt phòng
        public DateTime NgayNhanPhong { get; set; }
        public DateTime NgayTraPhong { get; set; }
        public int SoNgayO { get; set; }
        public decimal TongTien { get; set; }
        public int SoLuongNguoi { get; set; }

        // Thông tin Khách hàng
        public int TaiKhoanID { get; set; }
        public string HoTenKH { get; set; }
        public string EmailKH { get; set; }
        public string SDTKH { get; set; }

        // Ghi chú
        public string GhiChu { get; set; }
    }
}