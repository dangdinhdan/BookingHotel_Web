using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBooking_Web.Areas.Admin.ViewModel
{
    public class RevenueReportItem
    {
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int SoLuotDat { get; set; }
        public decimal DoanhThu { get; set; }
    }

    public class ReportResponse
    {
        public decimal TongDoanhThu { get; set; } // Hiển thị ô tổng
        public List<RevenueReportItem> ChiTiet { get; set; } // Dữ liệu cho bảng
        public List<decimal> ChartData { get; set; } // Mảng 12 phần tử cho biểu đồ
    }

}