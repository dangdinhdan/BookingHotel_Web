using HotelBooking_Web.Areas.Admin.ViewModel;
using HotelBooking_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelBooking_Web.Areas.Admin.Controllers
{
    public class ThongKeController : Controller
    {
        // GET: Admin/ThongKe
        private DataClasses1DataContext db = new DataClasses1DataContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetReportData(int? month, int? year)
        {
            
            // Nếu không chọn năm, mặc định lấy năm hiện tại để vẽ biểu đồ cho đẹp
            int selectedYear = year ?? DateTime.Now.Year;

            // Gọi Stored Procedure lấy danh sách chi tiết
            // Lưu ý: Cú pháp gọi SP phụ thuộc bạn dùng ADO.NET hay Entity Framework.
            // Dưới đây là ví dụ dùng EF:
            var rawData = db.sp_BaoCaoDoanhThuDatPhong(month, selectedYear).ToList();
            var dataList = rawData.Select(x => new RevenueReportItem
            {
                // Lưu ý: LINQ to SQL thường trả về int? (nullable), nên cần ép kiểu hoặc dùng ?? 0
                Thang = x.Thang ?? 0,
                Nam = x.Nam ?? 0,
                SoLuotDat = x.SoLuotDat ?? 0,
                DoanhThu = x.DoanhThu ?? 0
            }).ToList();

            // Gọi SP lấy Tổng doanh thu (hoặc có thể tính sum từ dataList ở trên để đỡ gọi DB 2 lần)
            // Ở đây tôi tính trực tiếp từ list cho nhanh và đồng bộ
            decimal totalRevenue = dataList.Sum(x => x.DoanhThu);

            // --- XỬ LÝ DỮ LIỆU CHO BIỂU ĐỒ (CHART) ---
            // Tạo mảng 12 phần tử mặc định bằng 0
            List<decimal> chartData = new List<decimal>();

            for (int i = 1; i <= 12; i++)
            {
                // Tìm xem tháng i có trong dữ liệu DB trả về không
                var item = dataList.FirstOrDefault(x => x.Thang == i);
                chartData.Add(item != null ? item.DoanhThu : 0);
            }

            // Đóng gói dữ liệu trả về
            var response = new ReportResponse
            {
                TongDoanhThu = totalRevenue,
                ChiTiet = dataList,
                ChartData = chartData
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

     
    }
}