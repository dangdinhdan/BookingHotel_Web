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
        public QLDPSevice service = new QLDPSevice();

        // GET: Admin/QLDP
        public ActionResult Index(int? page, string query, string status)
        {
            int pageIndex = page ?? 1;
            var pageSize = 10;
            var list = service.Search(query, status);
            var items = list.OrderByDescending(x => x.NgayNhanPhong).ToPagedList(pageIndex, pageSize);
            return View(items);
            
        }
        public ActionResult Detail(int id)
        {
            var item = db.vw_DanhSachDatPhongs.Where(o => o.DatPhongID == id && (o.isDelete == null||o.isDelete== false));
            return View(item);
        }
        public ActionResult Checkin()
        {
            return View();
        }
        public ActionResult Checkin_Comf()
        {
            return View();
        }
        public ActionResult Checkout()
        {
            return View();
        }
    }
}