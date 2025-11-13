using HotelBooking_Web.Areas.Admin.Service;
using HotelBooking_Web.Models;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelBooking_Web.Areas.Admin.Controllers
{
    public class QLLPhongController : Controller
    {
        private DataClasses1DataContext db = new DataClasses1DataContext();
        private QLLPhongService service = new QLLPhongService();
        // GET: Admin/QLLPhong
        public ActionResult Index(int ? page)
        {
            var pageSize = 10;
            int pageIndex = page ?? 1;
            var items = db.tbl_LoaiPhongs.Where(x=>x.isDelete == null || x.isDelete == false).OrderByDescending(x => x.LoaiPhongID).ToPagedList(pageIndex, pageSize);
            return View(items);
        }

        public ActionResult Them()
        {
            
            return View();
        }
        public ActionResult Sua(int id)
        {
            var item = db.tbl_LoaiPhongs.FirstOrDefault(x => x.LoaiPhongID == id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }
        

        public string Insert()
        {
            
            //lấy về mã lớp.

            string TenLoaiPhong_str = Request["txt_TenLoaiPhong"];
            string MoTa_str = Request["txt_MoTa"];

            //gọi hàm xử lý thêm mới vào db
            var qr = service.Them(TenLoaiPhong_str, MoTa_str);

            return JsonConvert.SerializeObject(qr);
        }

        public string Edit()
        {
            int LoaiPhongID = int.Parse(Request["Txt_LoaiPhongID"]);
            string TenLoaiPhong_str = Request["txt_TenLoaiPhong"];
            string MoTa_str = Request["txt_MoTa"];

            var qr = service.Sua(LoaiPhongID, TenLoaiPhong_str, MoTa_str);
            return JsonConvert.SerializeObject(qr);

        }

        [HttpPost]
        //public JsonResult Edit(int Txt_LoaiPhongID, string txt_TenLoaiPhong, string txt_MoTa)
        //{
        //    var result = service.Sua(Txt_LoaiPhongID, txt_TenLoaiPhong, txt_MoTa);
        //    return Json(result);
        //}



        //public bool Delete(int id)
        //{

        //    bool qr = service.Xoa(id);

        //    return qr;
        //}

        public string Delete(int id)
        {

            var qr = service.Xoa(id);

            return JsonConvert.SerializeObject(qr);
        }

    }
}