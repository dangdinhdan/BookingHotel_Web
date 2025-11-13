using HotelBooking_Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;


namespace HotelBooking_Web.Areas.Admin.Service
{

    public class QLPhongService
    {
        private static QLPhongService _instance;
        private static readonly object _lock = new object();

        // Constructor private ngăn tạo mới
        private QLPhongService() { }

        // Thuộc tính truy cập duy nhất
        public static QLPhongService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new QLPhongService();
                        }
                    }
                }
                return _instance;
            }
        }

        private DataClasses1DataContext db = new DataClasses1DataContext();
        public FunctResult<tbl_Phong> Them(string SoPhong, int LoaiPhongID, decimal GiaMoiDem, int SucChuaToiDa, string MoTa, string HinhAnh)
        {
            FunctResult<tbl_Phong> rs = new FunctResult<tbl_Phong>();

            try
            {
                //cố gắng lấy ra lớp quản lý có mã lớp là maLopQL
                var qr = db.tbl_Phongs.Where(o => o.SoPhong == SoPhong && (o.isDelete == null || o.isDelete == false));

                if (!qr.Any())
                {
                    //trường hợp chưa có lớp quản lý nào có mã lớp = maLopQL
                    tbl_Phong new_obj = new tbl_Phong();
                    new_obj.SoPhong = SoPhong;
                    new_obj.LoaiPhongID = LoaiPhongID;
                    new_obj.GiaMoiDem = GiaMoiDem;
                    new_obj.SucChuaToiDa = SucChuaToiDa;
                    new_obj.MoTa = MoTa;
                    new_obj.HinhAnh = HinhAnh;

                    new_obj.Create_at = DateTime.Now;

                    db.tbl_Phongs.InsertOnSubmit(new_obj);
                    db.SubmitChanges();

                    rs.ErrCode = EnumErrCode.Success;
                    rs.ErrDesc = "Thêm mới phòng thành công";

                }
                else
                {
                    //trường hợp đã tồn tại lớp quản lý có mã lớp = maLopQL
                    rs.ErrCode = EnumErrCode.Existent;
                    rs.ErrDesc = "Thêm mới phòng thất bại do đã tồn tại lớp quản lý có mã = " + SoPhong;

                }

            }
            catch (Exception ex)
            {
                //nếu lấy ds lớp quản lý lỗi thì trả ra fail
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDesc = "Có lỗi xảy ra trong quá trình thêm mới phòng. Chi tiết lỗi: " + ex.Message;

            }

            return rs;
        }



        public FunctResult<tbl_Phong> Sua(int id, string SoPhong, int LoaiPhongID, decimal GiaMoiDem, int SucChuaToiDa, string MoTa, string HinhAnh)
        {
            FunctResult<tbl_Phong> rs = new FunctResult<tbl_Phong>();

            try
            {
                //cố gắng lấy ra lớp quản lý có mã lớp là maLopQL
                var qr = db.tbl_Phongs.Where(o => o.PhongID == id && (o.isDelete == null || o.isDelete == false));

                if (qr.Any())
                {
                    //trường hợp lấy ra được dữ liệu lớp quản lý cần sửa
                    tbl_Phong old_obj = qr.SingleOrDefault();

                    old_obj.SoPhong = SoPhong ?? old_obj.SoPhong;
                    old_obj.LoaiPhongID = LoaiPhongID;
                    old_obj.GiaMoiDem = GiaMoiDem;
                    old_obj.SucChuaToiDa = SucChuaToiDa;
                    old_obj.MoTa = MoTa ?? old_obj.MoTa;
                    old_obj.HinhAnh = HinhAnh ?? old_obj.HinhAnh;
                    old_obj.Update_at = DateTime.Now;


                    db.SubmitChanges();


                    rs.ErrCode = EnumErrCode.Success;
                    rs.ErrDesc = "Chỉnh sửa thông tin phòng thành công";
                }
                else
                {
                    //trường hợp không tìm thấy lớp quản lý cần sửa

                    rs.ErrCode = EnumErrCode.NotExist;
                    rs.ErrDesc = "Không tìm thấy phòng cần sửa";
                }

            }
            catch (Exception ex)
            {
                //nếu lấy ds lớp quản lý lỗi thì trả ra fail
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDesc = "Có lỗi xảy ra trong quá trình chỉnh sửa dữ liệu phòng. Chi tiết lỗi: " + ex.Message;

            }

            return rs;
        }




        public PhongViewModel LayThongTinViewSua(int id)
        {
            var phong = db.tbl_Phongs.FirstOrDefault(x => x.PhongID == id);
            if (phong == null)
            {
                return null;
            }

            var loaiPhong = db.tbl_LoaiPhongs.FirstOrDefault(x => x.LoaiPhongID == phong.LoaiPhongID);
            var dsLoaiPhong = db.tbl_LoaiPhongs.Where(x => x.isDelete == null || x.isDelete == false).ToList();

            return new PhongViewModel
            {
                Phong = phong,
                LoaiPhong = loaiPhong,
                DSLP = dsLoaiPhong
            };
        }


        public FunctResult<tbl_Phong> Xoa(int id)
        {
            FunctResult<tbl_Phong> rs = new FunctResult<tbl_Phong>();
            try
            {
                var qr = db.tbl_Phongs.Where(o => o.PhongID == id && (o.isDelete == null || o.isDelete == false));
                if (qr.Any())
                {
                    tbl_Phong del_obj = qr.SingleOrDefault();
                    del_obj.isDelete = true;
                    del_obj.Delete_at = DateTime.Now;
                    db.SubmitChanges();
                    rs.ErrCode = EnumErrCode.Success;
                    rs.ErrDesc = "Xóa phòng thành công";

                }
                else
                {
                    rs.ErrCode = EnumErrCode.NotExist;
                    rs.ErrDesc = "không tìm thấy phòng cần xóa";

                }
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDesc = "có lỗi trong quá trình xóa phòng";

            }
            return rs;
        }
    

        public List<vw_DanhSachPhong> Search(string query)
        { 
            var rooms = db.vw_DanhSachPhongs.Where(x => x.isDelete == null || x.isDelete == false);

            if (!string.IsNullOrEmpty(query))
            {
                rooms = rooms.Where(x => x.SoPhong.Contains(query) || x.TenLoaiPhong.Contains(query)
                );
            }
            
            return rooms.ToList() ;

          
        }
    }
}