using HotelBooking_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBooking_Web.Areas.Admin.Service
{
    public class QLDPService
    {
        private DataClasses1DataContext db = new DataClasses1DataContext();
        public List<vw_DanhSachDatPhong> Search(string query,string status)
        {
            var list = db.vw_DanhSachDatPhongs.Where(x => x.isDelete == null || x.isDelete == false);
            if (!string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(query))
            {
                list = list.Where(x => x.TrangThai ==status &&(x.HoTen.Contains(query) || x.DatPhongID == int.Parse(query)));
                
            }
            if (string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(query))
            {
                list = list.Where(x => x.HoTen.Contains(query) || x.DatPhongID == int.Parse(query));

            }
            if (!string.IsNullOrEmpty(status) && string.IsNullOrEmpty(query))
            {
                list = list.Where(x => x.TrangThai == status);

            }



            return list.ToList();


        }

        public FunctResult<tbl_DatPhong> Checkin ( int DatPhongID)
        {
            FunctResult<tbl_DatPhong> rs = new FunctResult<tbl_DatPhong> ();
            try
            {
                var qr = db.tbl_DatPhongs.SingleOrDefault(o=>o.DatPhongID==DatPhongID);
                tbl_DatPhong DatPhong = qr;
                DatPhong.TrangThai = "Checkin";
                db.SubmitChanges();
                rs.ErrDesc = "Check in thành công";
                rs.ErrCode = EnumErrCode.Success;

            }
            catch (Exception ex)
            {
                rs.ErrDesc = "Có lỗi trong quá trình check in";
                rs.ErrCode = EnumErrCode.Error;
                
            }
            return rs;
        }

        
        public List<vw_DanhSachDatPhong> Search_DatPhong(int DatPhongID)
        {
            var Bookings = db.vw_DanhSachDatPhongs.Where(x => x.DatPhongID == DatPhongID && x.TrangThai == "Pending");
            return Bookings.ToList();
         
        }
    }
}