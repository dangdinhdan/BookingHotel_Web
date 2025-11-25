using HotelBooking_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace HotelBooking_Web.Areas.Admin.Controllers
{
    public class QLDPController : Controller
    {
        public DataClasses1DataContext db = new DataClasses1DataContext();

        // GET: Admin/QLDP
        public ActionResult Index(int? page)
        {
            int pageIndex = page ?? 1;
            var pageSize = 10;
            var item = db.vw_DanhSachDatPhongs.OrderByDescending(x => x.PhongID).ToPagedList(pageIndex, pageSize); ;
            //var item = db.vw_DanhSachDatPhongs.Where(o => o.isDelete == null || o.isDelete == false).ToList();
            return View(item);
        }
        public ActionResult Detail(int id)
        {
            var item = db.vw_DanhSachDatPhongs.Where(o => o.DatPhongID == id && (o.isDelete == null||o.isDelete== false));
            return View(item);
        }
        public ActionResult CheckIn()
        {
            return View();
        }
        public ActionResult CheckOut()
        {
            return View();
        }
    }
}