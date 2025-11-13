using HotelBooking_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;

namespace HotelBooking_Web.Areas.Admin.Service
{
    public class QLLPhongService
    {
       private DataClasses1DataContext db = new DataClasses1DataContext();
       public FunctResult<tbl_LoaiPhong> Them(string TenLoaiPhong,string MoTa)
        {
            FunctResult<tbl_LoaiPhong> rs = new FunctResult<tbl_LoaiPhong>();
            try
            {
                var qr = db.tbl_LoaiPhongs.Where(o=>o.TenLoaiPhong == TenLoaiPhong &&(o.isDelete==null || o.isDelete==false));
                if (!qr.Any()) {
                    tbl_LoaiPhong new_obj = new tbl_LoaiPhong();
                    new_obj.TenLoaiPhong= TenLoaiPhong;
                    new_obj.MoTa= MoTa;
                    new_obj.Create_at= DateTime.Now;
                    db.tbl_LoaiPhongs.InsertOnSubmit(new_obj);
                    db.SubmitChanges();


                    rs.ErrCode = EnumErrCode.Success;
                    rs.ErrDesc = "Thêm mới loại phòng thành công";
                    rs.Data = new_obj;
                }
                else
                {
                    rs.ErrCode = EnumErrCode.Existent;
                    rs.ErrDesc = "Thêm mới lớp quản lý thất bại do đã tồn tại lớp quản lý có mã = " + TenLoaiPhong;
                    rs.Data = null;
                }
            }
            catch(Exception ex) 
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDesc = "Có lỗi xảy ra trong quá trình thêm mới Loại phòng. Chi tiết lỗi: " + ex.Message;
                rs.Data = null;
            }
            return rs;
        }


        public FunctResult<tbl_LoaiPhong> Sua(int id,String TenLoaiPhong, string MoTa)
        {
            FunctResult<tbl_LoaiPhong> rs = new FunctResult<tbl_LoaiPhong>();
            try
            {
                var qr = db.tbl_LoaiPhongs.Where(o => o.LoaiPhongID == id &&(o.isDelete == null || o.isDelete == false));
                if (qr.Any())
                {
                    tbl_LoaiPhong old_obj = qr.SingleOrDefault();
                    old_obj.TenLoaiPhong = TenLoaiPhong ?? old_obj.TenLoaiPhong;
                    old_obj.MoTa = MoTa ?? old_obj.MoTa;
                    old_obj.Update_at = DateTime.Now;

                    db.SubmitChanges();
                    rs.ErrCode = EnumErrCode.Success;
                    rs.ErrDesc = "cập nhật thành công";
                    
                }
                else 
                {
                    
                    rs.ErrCode = EnumErrCode.Empty;
                    rs.ErrDesc = "không tìm thấy loại phòng cần sửa";

                }
            }
            catch (Exception ex) 
            { 
                
                rs.ErrCode=EnumErrCode.Error;
                rs.ErrDesc = "có lỗi trong quá trình sửa loại phòng";
            }
            return rs;

        }

        //public bool Xoa(int ID)
        //{
        //    try
        //    {
        //        var qr = db.tbl_LoaiPhongs.Where(o => o.LoaiPhongID == ID && (o.isDelete == null || o.isDelete == false));

        //        if (qr.Any())
        //        {
        //            tbl_LoaiPhong del_obj = qr.SingleOrDefault();
        //            del_obj.isDelete = true;
        //            del_obj.Delete_at = DateTime.Now;

        //            db.SubmitChanges();
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        public FunctResult<tbl_LoaiPhong> Xoa(int ID)
        {
            FunctResult<tbl_LoaiPhong> rs = new FunctResult<tbl_LoaiPhong>();
            try
            {
                var qr = db.tbl_LoaiPhongs.Where(o => o.LoaiPhongID == ID && (o.isDelete == null || o.isDelete == false));
                if (qr.Any())
                {
                    tbl_LoaiPhong del_obj = qr.SingleOrDefault();
                    del_obj.isDelete = true;
                    del_obj.Delete_at = DateTime.Now;
                    db.SubmitChanges();
                    rs.ErrCode = EnumErrCode.Success;
                    rs.ErrDesc = "Xóa loại phòng thành công";
                    
                }
                else
                {
                    rs.ErrCode = EnumErrCode.NotExist;
                    rs.ErrDesc = "không tìm thấy loại phòng cần xóa";
                    
                }
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDesc = "có lỗi trong quá trình xóa phòng";
                
            }
            return rs;
        }
    }
    
}