using HotelBooking_Web.Models;
using HotelBooking_Web.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace HotelBooking_Web.Controllers
{
    public class BookingController : Controller
    {
        private DataClasses1DataContext db = new DataClasses1DataContext();
        private BookingService service = new BookingService();
        // GET: Booking
        public ActionResult Index(DatPhongViewModel model)
        {
            var item = db.tbl_Phongs.SingleOrDefault(o => o.PhongID == model.PhongID);
            ViewBag.Phong = item;
            
            decimal TongTienPhong = model.SoDem * item.GiaMoiDem;
            ViewBag.TongTienPhong = TongTienPhong;
            decimal ThuePhi = TongTienPhong * 0.03m ;
            ViewBag.ThuePhi = ThuePhi;
            decimal TongTien = TongTienPhong + ThuePhi;
            ViewBag.TongTien = TongTien;

            return View(model);
        }
        public ActionResult DatPhong()
        {
            int TaiKhoanID =int.Parse(Session["TaiKhoanID"].ToString());
            int PhongID = int.Parse(Request["txt_PhongID"]);
            DateTime Checkin = DateTime.Parse(Request["txt_CheckIn"]);
            DateTime Checkout = DateTime.Parse(Request["txt_CheckOut"]);
            int SoNguoi = int.Parse(Request["txt_SoNguoi"]);
            decimal TongTien = decimal.Parse(Request["txt_TongTien"]);
            string GhiChu = Request["txt_GhiChu"];
            var qr = service.DatPhong(TaiKhoanID, PhongID, Checkin, Checkout, SoNguoi, TongTien, GhiChu);
            return Json(qr);

        }

    }
}