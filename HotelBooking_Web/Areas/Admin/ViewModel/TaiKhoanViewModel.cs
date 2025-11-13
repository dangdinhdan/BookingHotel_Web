using HotelBooking_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBooking_Web.Areas.Admin.ViewModel
{
    public class TaiKhoanViewModel
    {
        public vw_DanhSachTaiKhoan taikhoan {get; set;}
        public List<tbl_VaiTro> DSVT {get; set;}
    }
}