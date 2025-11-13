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
        private QLPhongService service = QLPhongService.Instance;

        public ActionResult Index(string query, int? page)
        {
            var pageSize = 10;
            int pageIndex = page ?? 1;

            var rooms = service.Search(query);

            var items = rooms
                .OrderByDescending(x => x.PhongID).ToPagedList(pageIndex, pageSize);

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


            return View(model);
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
        public string Edit(HttpPostedFileBase txt_HinhAnh)
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
                string path = Path.Combine(Server.MapPath("~/Assets/img/credit"), fileName);

                // Lưu file vật lý
                txt_HinhAnh.SaveAs(path);

                // Lưu đường dẫn vào DB
                HinhAnh_str = "/Assets/img/credit/" + fileName;
            }

            var qr = service.Sua(PhongID_int,SoPhong_str, LoaiPhong_int, GiaMoiDem_del, SucChuaToiDa_int, MoTa_str, HinhAnh_str);
            return JsonConvert.SerializeObject(qr);
        }

        public string Delete(int id)
        {

            var qr = service.Xoa(id);

            return JsonConvert.SerializeObject(qr);
        }
    }

}
