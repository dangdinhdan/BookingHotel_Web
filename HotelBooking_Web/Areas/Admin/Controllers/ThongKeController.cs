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
            if (Session["Ad_ID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "TaiKhoan");
            }
        }

        [HttpGet]
        public JsonResult GetReportData(int? month, int? year)
        {
            int selectedYear = year ?? DateTime.Now.Year;

            
            var rawData = db.sp_BaoCaoDoanhThuDatPhong(month, selectedYear).ToList();
            var dataList = rawData.Select(x => new ThongKeViewModel
            {
                Thang = x.Thang ?? 0,
                Nam = x.Nam ?? 0,
                SoLuotDat = x.SoLuotDat ?? 0,
                DoanhThu = x.DoanhThu ?? 0
            }).ToList();

           
            decimal totalRevenue = dataList.Sum(x => x.DoanhThu);

            // --- XỬ LÝ DỮ LIỆU CHO BIỂU ĐỒ (CHART) ---
            // Tạo mảng 12 phần tử mặc định bằng 0
            List<decimal> chartData = new List<decimal>();

            for (int i = 1; i <= 12; i++)
            {
                
                var item = dataList.FirstOrDefault(x => x.Thang == i);
                chartData.Add(item != null ? item.DoanhThu : 0);
            }

            // Đóng gói dữ liệu trả về
            var response = new KetQuaThongKe
            {
                TongDoanhThu = totalRevenue,
                ChiTiet = dataList,
                ChartData = chartData
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

     
    }
}