using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBooking_Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    namespace HotelBooking_Web.Controllers
    {
        public class RoomController : Controller
        {
            public ActionResult Rooms()
            {
                return View();
            }

            public ActionResult Room(int id)
            {
                ViewBag.RoomId = id;
                return View();
            }
        }
    }
}