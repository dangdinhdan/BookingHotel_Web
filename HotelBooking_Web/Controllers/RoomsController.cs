
﻿    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using HotelBooking_Web.Models;

namespace HotelBooking_Web.Controllers
{
        public class RoomsController : Controller
        {
                private DataClasses1DataContext db = new DataClasses1DataContext(System.Configuration.ConfigurationManager.ConnectionStrings["QLKSLenh"].ConnectionString);
            // GET: Rooms

            public ActionResult SearchRooms(DateTime? checkin, DateTime? checkout, int? guests)
            {
                    if (checkin == null) checkin = DateTime.Now;
                    if (checkout == null) checkout = DateTime.Now.AddDays(1);
                    if (guests == null) guests = 1;

                    ViewBag.CheckIn = checkin.Value.ToString("yyyy-MM-dd");
                    ViewBag.CheckOut = checkout.Value.ToString("yyyy-MM-dd");
                    ViewBag.Guests = guests;

                    // Gọi Procedure
                    var danhSachPhong = db.sp_TimPhongTrong(checkin, checkout, guests).ToList();

                    return View(danhSachPhong);
            }
            

                public ActionResult Detail(int id)
                {
                    var Phong= db.tbl_Phongs.FirstOrDefault(p => p.PhongID == id);
                    if (Phong == null)
                    {
                        return HttpNotFound();
                    }
                            return View(Phong);
                }

           
        }

}

