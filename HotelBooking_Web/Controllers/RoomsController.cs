
    using HotelBooking_Web.Models;
﻿    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

namespace HotelBooking_Web.Controllers
{
        public class RoomsController : Controller
        {

            private DataClasses1DataContext db = new DataClasses1DataContext();


            public ActionResult SearchRooms(DateTime? checkin, DateTime? checkout, int? guests, int? priceRange, int? loaiPhongID)
            {
                    if (checkin == null) checkin = DateTime.Now;
                    if (checkout == null) checkout = DateTime.Now.AddDays(1);
                    if (guests == null) guests = 1;

                    ViewBag.CheckIn = checkin.Value.ToString("yyyy-MM-dd");
                    ViewBag.CheckOut = checkout.Value.ToString("yyyy-MM-dd");
                    ViewBag.Guests = guests;
                    ViewBag.PriceRange = priceRange;
                    ViewBag.SelectedLoaiPhongID = loaiPhongID;
                    ViewBag.LoaiPhongs = db.tbl_LoaiPhongs.ToList();
                    

            // Gọi Procedure
                var danhSachPhong = db.sp_TimPhongTrong(checkin, checkout, guests).ToList();

                var danhSachLoai = db.tbl_LoaiPhongs.ToList();
                    ViewBag.LoaiPhongs = danhSachLoai;

                    if (priceRange != null)
                    {
                        switch (priceRange)
                        {
                            case 1: // duoi 5 lop
                                danhSachPhong = danhSachPhong.Where(p => p.GiaMoiDem <= 500000).ToList();
                                break;
                            case 2: // duoi 1 cu
                                danhSachPhong = danhSachPhong.Where(p => p.GiaMoiDem <= 1000000).ToList();
                                break;
                            case 3: // duoi 3 cu
                                danhSachPhong = danhSachPhong.Where(p => p.GiaMoiDem <= 3000000).ToList();
                                break;
                            case 4: //tren 3 cu
                                danhSachPhong = danhSachPhong.Where(p => p.GiaMoiDem > 3000000).ToList();
                                break;
                        }
                    }

                    if (loaiPhongID != null)
                    {
                        danhSachPhong = danhSachPhong.Where(p => p.LoaiPhongID == loaiPhongID).ToList();
                    }

                return View(danhSachPhong);
            }

            public ActionResult Detail(int id)
            {
                ViewBag.PhongID = id;
                return View();

            }

        [HttpGet]
            public JsonResult GetRoomDetailJson(int id) // 'id'
            {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                //db.DeferredLoadingEnabled = false;

                // Tìm phòng theo PhongID (Kiểu int)
                var room = db.tbl_Phongs.FirstOrDefault(x => x.PhongID == id);

                if (room == null) return Json(new { success = false , message = "Không thấy phòng " + id + " trong Database: " + db.Connection.Database}, JsonRequestBehavior.AllowGet);

                    var listImages = room.tbl_PhongImages.Select(img => img.Url).ToList();

                    // Nếu không có ảnh phụ, lấy ảnh chính
                    if (listImages.Count == 0 && !string.IsNullOrEmpty(room.HinhAnh))
                    {
                        listImages.Add(room.HinhAnh);
                    }
                    // Nếu vẫn không có, lấy ảnh placeholder
                    if (listImages.Count == 0) listImages.Add("/Images/room-placeholder.png");

                    var data = new
                    {
                        success = true,
                        TenPhong = room.SoPhong,
                        LoaiPhong = room.tbl_LoaiPhong.TenLoaiPhong,
                        Gia = room.GiaMoiDem,
                        MoTa = room.MoTa,
                        DanhSachAnh = listImages
                    };

                    return Json(data, JsonRequestBehavior.AllowGet);
            }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Lỗi Server: " + ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

}

