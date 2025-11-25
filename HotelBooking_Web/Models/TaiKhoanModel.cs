using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBooking_Web.Models
{
    public class TaiKhoanModel
    {
        public int TaiKhoanID { get; set; }

        // MaTK (computed column) — bạn không set giá trị thủ công
        public string MaTK { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string MatKhau { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public DateTime Create_at { get; set; }
        public int VaiTroID { get; set; }
        public DateTime? Update_at { get; set; }
        public bool isDelete { get; set; }
        public DateTime? Delete_at { get; set; }
    }
}