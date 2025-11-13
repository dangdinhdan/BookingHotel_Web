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

}