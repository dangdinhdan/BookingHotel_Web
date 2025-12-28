using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBooking_Web.Models
{
    public class PhongViewModel
    {
        public tbl_Phong Phong { get; set; }
        public tbl_LoaiPhong LoaiPhong { get; set; }
        public IEnumerable<tbl_LoaiPhong> DSLP { get; set; }
    }


    public class chitietphong
    {
        public PhongViewModel ThongTinPhong { get; set; }
        public IEnumerable<tbl_PhongImage> DSAnh { get; set; }
    }

    public class ThongTinPhong
    {
        public string SoPhong { get; set; }
        public int LoaiPhongID { get; set; }
        public decimal GiaMoiDem { get; set; }
        public int? SucChuaToiDa { get; set; }
        public string MoTa { get; set; }
        public string HinhAnh {  get; set; }
    }
}
