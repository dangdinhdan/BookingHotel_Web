using HotelBooking_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Newtonsoft.Json;
using System.IO;
using System.Net.NetworkInformation;
using HotelBooking_Web.Areas.Admin.Service;


namespace HotelBooking_Web.Areas.Admin.Controllers
{
    public class QLDPController : Controller
    {
        public DataClasses1DataContext db = new DataClasses1DataContext();
        public QLDPService service = new QLDPService();

        // GET: Admin/QLDP
        public ActionResult Index(int? page, string query, string status)
        {
            
            if (Session["Ad_ID"] != null)
            {
                int pageIndex = page ?? 1;
                var pageSize = 10;
                var list = service.Search(query, status);
                var items = list.OrderByDescending(x => x.DatPhongID).ToPagedList(pageIndex, pageSize);
                return View(items);
            }
            else
            {
                return RedirectToAction("Login", "TaiKhoan");
            }

        }
        public ActionResult Detail(int id)
        {
            var item = db.vw_DanhSachDatPhongs.SingleOrDefault(o => o.DatPhongID == id && (o.isDelete == null||o.isDelete== false));
            return View(item);
        }
        public ActionResult Checkin()
        {
            return View(); 

        }

        public JsonResult Checkin_search(int DatPhongID)
        {
            var DatPhong = service.Search_DatPhong(DatPhongID);

            if (DatPhong.Any())
            {
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = false, message = "Mã đặt phòng không tồn tại!" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Checkin_Comf(int DatPhongID)
        {
            
            var item = db.vw_DanhSachDatPhongs.SingleOrDefault(o=>o.DatPhongID==DatPhongID);
            
            return View(item);
        }
        public string Checkin_Action(int DatPhongID)
        {
            var qr = service.Checkin(DatPhongID);
            
            return JsonConvert.SerializeObject(qr);
        }



        public ActionResult Checkout(int id)
        {
            var item = db.vw_DanhSachDatPhongs.SingleOrDefault(o => o.DatPhongID == id);
            return View(item);
        }
        public string Checkout_Action()
        {
            int DatPhongID = int.Parse(Request["txt_DatPhongID"]);
            string txt_PhuongThuc = Request["slc_PhuongThuc"];
            string PhuongThuc = null;
            if (txt_PhuongThuc == "TienMat")
            {
                PhuongThuc = "Tiền mặt";
            }
            else
            {
                PhuongThuc = "Chuyển khoản";
            }


            var rs = service.checkout(DatPhongID, PhuongThuc);
            return JsonConvert.SerializeObject(rs);
        }
    }
}