using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelBooking_Web.Controllers
    {
        public class RoomsController : Controller
        {
            public ActionResult SearchRooms(DateTime? checkin, DateTime? checkout, int? guests)
            {
                //ViewBag.CheckinDate = checkin;
                //ViewBag.CheckoutDate = checkout;
                //ViewBag.GuestCount = guests;

                return View();
            }

            public ActionResult Detail()
            {
                return View();
            }

           
        }
    }
