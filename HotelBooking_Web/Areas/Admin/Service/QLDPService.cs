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

                tbl_DatPhong qr_dp = db.tbl_DatPhongs.SingleOrDefault(o => o.DatPhongID == DatPhongID);
                
                tbl_Phong qr_p = db.tbl_Phongs.SingleOrDefault(o => o.PhongID == qr_dp.PhongID);
                
                tbl_GiaoDich newGiaoDich = new tbl_GiaoDich();
                if (qr_dp.NgayNhanPhong.Date == DateTime.Now.Date)
                {
                    newGiaoDich.DatPhongID = DatPhongID;
                    newGiaoDich.SoTien = qr_dp.TongTien;
                    newGiaoDich.TrangThai = "Unpaid";
                    newGiaoDich.Create_at = DateTime.Now;

                    qr_p.TrangThai = "Occupied";
                    qr_dp.TrangThai = "Checkin";
                    db.tbl_GiaoDiches.InsertOnSubmit(newGiaoDich);
                    db.SubmitChanges();
                    rs.ErrDesc = "Check in thành công";
                    rs.ErrCode = EnumErrCode.Success;
                }
                else if (qr_dp.NgayNhanPhong.Date < DateTime.Now.Date)
                {
                    qr_dp.TrangThai = "Canceled";
                    db.SubmitChanges();
                    rs.ErrDesc = "Đã quá ngày check in, đặt phòng bị hủy";
                    rs.ErrCode = EnumErrCode.Fail;
                }
                else
                {
                    rs.ErrDesc = "Check in thất bại vì hôm nay ko phải ngày nhận phòng";
                    rs.ErrCode = EnumErrCode.Fail;
                }
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

        public FunctResult<tbl_GiaoDich> checkout (int DatPhongID, string PhuongThuc)
        {
            FunctResult<tbl_GiaoDich> rs = new FunctResult<tbl_GiaoDich>();
            try
            {
                tbl_GiaoDich qr_gd = db.tbl_GiaoDiches.SingleOrDefault(o => o.DatPhongID == DatPhongID);
                tbl_DatPhong qr_dp = db.tbl_DatPhongs.SingleOrDefault(o => o.DatPhongID == DatPhongID);
                
                tbl_Phong qr_p = db.tbl_Phongs.SingleOrDefault(o => o.PhongID == qr_dp.PhongID);
                if(qr_gd != null && qr_dp != null)
                {
                    
                    
                    qr_gd.TrangThai = "Paid";
                    qr_gd.Update_at = DateTime.Now;
                    qr_dp.TrangThai = "Checkout";
                    qr_p.TrangThai = "Available";
                    qr_gd.PhuongThuc = PhuongThuc;

                    db.SubmitChanges();

                    rs.ErrDesc = "Check out thành công";
                    rs.ErrCode = EnumErrCode.Success;
                    

                }
                else
                {
                    rs.ErrDesc = "Không tìm thấy giao dịch";
                    rs.ErrCode = EnumErrCode.Error;
                }
            }
            catch(Exception ex)
            {
                rs.ErrDesc = "Có lỗi trong quá trình check out";
                rs.ErrCode = EnumErrCode.Error;

            }
            return rs;
        }
    }
}