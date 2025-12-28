using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBooking_Web.Areas.Admin.ViewModel
{
    public class ThongKeViewModel
    {
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int SoLuotDat { get; set; }
        public decimal DoanhThu { get; set; }
    }

    public class KetQuaThongKe
    {
        public decimal TongDoanhThu { get; set; } // Hiển thị ô tổng
        public List<ThongKeViewModel> ChiTiet { get; set; } // Dữ liệu cho bảng
        public List<decimal> ChartData { get; set; } // Mảng 12 phần tử cho biểu đồ
    }

}