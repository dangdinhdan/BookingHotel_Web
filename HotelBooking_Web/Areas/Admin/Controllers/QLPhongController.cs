using HotelBooking_Web.Areas.Admin.Service;
using HotelBooking_Web.Models;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;

namespace HotelBooking_Web.Areas.Admin.Controllers
{
    public class QLPhongController : Controller
    {
        // GET: Admin/QLPhong
        private DataClasses1DataContext db = new DataClasses1DataContext();
        private QLPhongService service = new QLPhongService();

        public ActionResult Index(string query, int? page)
        {
            var pageSize = 10;
            int pageIndex = page ?? 1;

            var rooms = service.Search(query);

            var items = rooms.OrderByDescending(x => x.PhongID).ToPagedList(pageIndex, pageSize);

            return View(items);
        }

        public ActionResult Them()
        {
            var items = db.tbl_LoaiPhongs.Where(x => x.isDelete == null || x.isDelete == false);
            return View(items);
        }

        public ActionResult Sua(int id)
        {
            var model = service.LayThongTinViewSua(id);

            var item = new chitietphong
            {
                ThongTinPhong = model,
                DSAnh = db.tbl_PhongImages.Where(o=>o.PhongID == id)
            };

            return View(item);
        }

        

        //public string Insert()
        //{
        //    string SoPhong_str = Request["txt_SoPhong"];
        //    int LoaiPhong_int = int.Parse(Request["slc_LoaiPhong"]);
        //    decimal GiaMoiDem_del = decimal.Parse(Request["txt_GiaMoiDem"]);
        //    int SucChuaToiDa_int = int.Parse(Request["txt_SucChuaToiDa"]);
        //    string MoTa_str = Request["txt_MoTa"];
        //    string HinhAnh_str = null;

        //    var qr = service.Them(SoPhong_str, LoaiPhong_int, GiaMoiDem_del, SucChuaToiDa_int, MoTa_str, HinhAnh_str);
        //    return JsonConvert.SerializeObject(qr);

        //}
        [HttpPost]
        public string Insert(HttpPostedFileBase txt_HinhAnh)
        {

            string SoPhong_str = Request["txt_SoPhong"];
            int LoaiPhong_int = int.Parse(Request["slc_LoaiPhong"]);
            decimal GiaMoiDem_del = decimal.Parse(Request["txt_GiaMoiDem"]);
            int SucChuaToiDa_int = int.Parse(Request["txt_SucChuaToiDa"]);
            string MoTa_str = Request["txt_MoTa"];
            string HinhAnh_str = null;

            if (txt_HinhAnh != null && txt_HinhAnh.ContentLength > 0)
            {
                // Lấy tên file
                string fileName = Path.GetFileName(txt_HinhAnh.FileName);

                // Đường dẫn lưu trên server
                string path = Path.Combine(Server.MapPath("~/Assets/img/credit"), fileName);

                // Lưu file vật lý
                txt_HinhAnh.SaveAs(path);

                // Lưu đường dẫn vào DB
                HinhAnh_str = "/Assets/img/credit/" + fileName;
            }

            var qr = service.Them(SoPhong_str, LoaiPhong_int, GiaMoiDem_del, SucChuaToiDa_int, MoTa_str, HinhAnh_str);
            return JsonConvert.SerializeObject(qr);
        }


        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase txt_HinhAnh)
        {
            int PhongID_int = int.Parse(Request["txt_PhongID"]);
            string SoPhong_str = Request["txt_SoPhong"];
            int LoaiPhong_int = int.Parse(Request["slc_LoaiPhong"]);
            decimal GiaMoiDem_del = decimal.Parse(Request["txt_GiaMoiDem"]);
            int SucChuaToiDa_int = int.Parse(Request["txt_SucChuaToiDa"]);
            string MoTa_str = Request["txt_MoTa"];
            string HinhAnh_str = null;

            if (txt_HinhAnh != null && txt_HinhAnh.ContentLength > 0)
            {
                // Lấy tên file
                string fileName = Path.GetFileName(txt_HinhAnh.FileName);

                // Đường dẫn lưu trên server
                string path = Path.Combine(Server.MapPath("~/Assets/img/rooms"), fileName);

                // Lưu file vật lý
                txt_HinhAnh.SaveAs(path);

                // Lưu đường dẫn vào DB
                HinhAnh_str = "/Assets/img/rooms/" + fileName;
            }

            var qr = service.Sua(PhongID_int,SoPhong_str, LoaiPhong_int, GiaMoiDem_del, SucChuaToiDa_int, MoTa_str, HinhAnh_str);
            return Json(qr);
        }

        public string Delete(int id)
        {

            var qr = service.Xoa(id);

            return JsonConvert.SerializeObject(qr);
        }



        [HttpPost]
        public ActionResult UploadImages(int PhongID, IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                if (files == null || !files.Any())
                {
                    return Json(new { success = false, message = "Không có file nào được tải lên!" });
                }

                string folderPath = Server.MapPath("~/Assets/img/rooms");

                // Tạo thư mục nếu chưa có
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        // Kiểm tra loại file cho an toàn
                        var ext = Path.GetExtension(file.FileName).ToLower();

                        string[] allowExt = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

                        if (!allowExt.Contains(ext))
                        {
                            return Json(new
                            {
                                success = false,
                                message = "File không hợp lệ! Chỉ chấp nhận jpg, png, gif."
                            });
                        }

                        // Tạo tên file duy nhất
                        string fileName = Guid.NewGuid().ToString() + ext;
                        string path = Path.Combine(folderPath, fileName);

                        // Lưu file vào thư mục
                        file.SaveAs(path);

                        // Lưu vào DB
                        tbl_PhongImage img = new tbl_PhongImage
                        {
                            PhongID = PhongID,
                            Url = "/Assets/img/rooms/" + fileName
                        };

                        db.tbl_PhongImages.InsertOnSubmit(img);
                    }
                }

                db.SubmitChanges();

                return Json(new { success = true, message = "Upload ảnh thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }


        [HttpPost]
        public JsonResult DeleteImage(int id)
        {
            var img = db.tbl_PhongImages.SingleOrDefault(x => x.PhongImages == id);

            if (img == null)
            {
                return Json(new { status = false });
            }

            
            string fullPath = Server.MapPath(img.Url);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            
            db.tbl_PhongImages.DeleteOnSubmit(img);
            db.SubmitChanges();

            return Json(new { status = true });
        }

    }

}
