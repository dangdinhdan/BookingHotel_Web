using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelBooking_Web.Areas.Admin.Controllers
{
    public class QLDPController : Controller
    {
        // GET: Admin/QLDP
        public ActionResult Index()
        {
            return View();
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