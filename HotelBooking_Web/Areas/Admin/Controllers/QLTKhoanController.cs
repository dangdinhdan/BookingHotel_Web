using HotelBooking_Web.Areas.Admin.Service;
using HotelBooking_Web.Models;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace HotelBooking_Web.Areas.Admin.Controllers
{
    public class QLTKhoanController : Controller
    {
        private DataClasses1DataContext db = new DataClasses1DataContext();
        private QLTKhoanService service = new QLTKhoanService();
        // GET: Admin/QLTKhoan
        public ActionResult Index(string query,int? page)
        {
            var pageSize = 10;
            int pageIndex = page ?? 1;

            var list = service.Search(query);

            var items = list.OrderByDescending(x => x.TaiKhoanID).ToPagedList(pageIndex, pageSize);

            return View(items);
        }

        public ActionResult Them()
        {
            var items = db.tbl_VaiTros.Where(x => x.isDelete == null || x.isDelete == false);
            return View(items);
        }

        public ActionResult Sua(int id)
        {
            var items = service.LayThongTinViewSua(id);

            return View(items);
        }

        public string Insert()
        {
            
            string txt_HoTen = Request["txt_HoTen"];
            string txt_Email = Request["txt_Email"];
            string txt_SoDienThoai = Request["txt_SoDienThoai"];
            string txt_DiaChi = Request["txt_DiaChi"];
            string txt_MatKhau = Request["txt_MatKhau"];
            int slc_VaiTro = int.Parse(Request["slc_VaiTro"]);
            var qr = service.Them(txt_HoTen, txt_DiaChi, txt_Email, txt_SoDienThoai, txt_MatKhau, slc_VaiTro);
            return JsonConvert.SerializeObject(qr);
        }

        public string Delete(int id)
        {

            var qr = service.Xoa(id);

            return JsonConvert.SerializeObject(qr);
        }

        public string Edit()
        {
            int txt_TaiKhoanID = int.Parse(Request["txt_TaiKhoanID"]);
            string txt_HoTen = Request["txt_HoTen"];
            string txt_Email = Request["txt_Email"];
            string txt_SoDienThoai = Request["txt_SoDienThoai"];
            string txt_DiaChi = Request["txt_DiaChi"];
            string txt_MatKhau = Request["txt_MatKhau"];
            int slc_VaiTro = int.Parse(Request["slc_VaiTro"]);
            var qr = service.Sua(txt_TaiKhoanID, txt_HoTen, txt_DiaChi, txt_Email, txt_SoDienThoai, txt_MatKhau, slc_VaiTro);

            return JsonConvert.SerializeObject(qr);
        }
    }
}