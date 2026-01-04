using HotelBooking_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBooking_Web.Service
{
    public class BookingService
    {
        private DataClasses1DataContext db = new DataClasses1DataContext();

        public FunctResult<tbl_DatPhong> DatPhong(int TaiKhoanID, int PhongID, DateTime Checkin, DateTime Checkout, int SoNguoi, decimal TongTien, string GhiChu)
        {
            FunctResult<tbl_DatPhong> rs = new FunctResult<tbl_DatPhong>();
            try
            {
                var qr = db.tbl_TaiKhoans.SingleOrDefault(o => o.TaiKhoanID == TaiKhoanID);
                if (qr != null)
                {
                    tbl_DatPhong new_obj = new tbl_DatPhong();
                    new_obj.TaiKhoanID = TaiKhoanID;
                    new_obj.PhongID = PhongID;
                    new_obj.NgayDat = DateTime.Today;
                    new_obj.NgayNhanPhong = Checkin;
                    new_obj.NgayTraPhong = Checkout;
                    new_obj.SoLuongNguoi = SoNguoi;
                    new_obj.TongTien = TongTien;
                    new_obj.GhiChu = GhiChu;
                    new_obj.TrangThai = "Pending";
                    db.tbl_DatPhongs.InsertOnSubmit(new_obj);
                    db.SubmitChanges();


                    rs.ErrCode = EnumErrCode.Success;
                    rs.ErrDesc = "Đặt phòng thành công, mã đặt phòng của bạn là " + new_obj.DatPhongID;

                }
                else
                {
                    rs.ErrCode = EnumErrCode.NotExist;
                    rs.ErrDesc = "không thể đặt phòng vì tài khoản không tồn tại";

                }
            }
            catch (Exception ex)
            {
                rs.ErrDesc = "Có lỗi trong quá trình Đặt phòng" + ex;
                rs.ErrCode = EnumErrCode.Error;

            }
            return rs;
        }
    }
}