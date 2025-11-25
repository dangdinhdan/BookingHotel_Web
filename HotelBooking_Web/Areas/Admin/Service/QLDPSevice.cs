using HotelBooking_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBooking_Web.Areas.Admin.Service
{
    public class QLDPSevice
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

    }
}