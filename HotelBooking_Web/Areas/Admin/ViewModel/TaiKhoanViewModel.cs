using HotelBooking_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBooking_Web.Areas.Admin.ViewModel
{
    public class TaiKhoanViewModel
    {
        public tbl_TaiKhoan taikhoan {get; set;}
        public List<tbl_VaiTro> DSVT {get; set;}


    }

    public class ThongTinTaiKhoan
    {
        public int TaiKhoanID { get; set; }
        public string MaTK { get; set; }

        public string HoTen { get; set; }
        public string Email { get; set; }
        
        public string VaiTro { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
    }
}