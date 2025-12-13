using HotelBooking_Web.Areas.Admin.ViewModel;
using HotelBooking_Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Helpers;

namespace HotelBooking_Web.Areas.Admin.Service
{
    public class TaiKhoanService
    {
        private DataClasses1DataContext db = new DataClasses1DataContext();


        public FunctResult<tbl_TaiKhoan> Them(string HoTen, string DiaChi, string Email,string SoDienThoai, string MatKhau,string VaiTro)
        {
            FunctResult<tbl_TaiKhoan> rs = new FunctResult<tbl_TaiKhoan>();

            try
            {
                //cố gắng lấy ra tài khoản có email là 
                var qr = db.tbl_TaiKhoans.Where(o => o.Email == Email);

                if (!qr.Any())
                {
                    //trường hợp chưa có email tồn tại
                    tbl_TaiKhoan new_obj = new tbl_TaiKhoan();
                    new_obj.HoTen = HoTen;
                    new_obj.DiaChi = DiaChi;
                    new_obj.Email = Email;
                    new_obj.SoDienThoai = SoDienThoai;
                    new_obj.MatKhau = MatKhau;
                    new_obj.VaiTro = VaiTro;

                    new_obj.Create_at = DateTime.Now;

                    db.tbl_TaiKhoans.InsertOnSubmit(new_obj);
                    db.SubmitChanges();

                    rs.ErrCode = EnumErrCode.Success;
                    rs.ErrDesc = "Thêm mới thành công";

                }
                else
                {
                    if (qr.SingleOrDefault().isDelete == true)
                    {
                        tbl_TaiKhoan old_obj = qr.SingleOrDefault();

                        old_obj.HoTen = HoTen ?? old_obj.HoTen;
                        old_obj.Email = Email;
                        old_obj.SoDienThoai = SoDienThoai;
                        old_obj.MatKhau = MatKhau ?? old_obj.MatKhau;
                        old_obj.DiaChi = DiaChi ?? old_obj.DiaChi;
                        old_obj.VaiTro = VaiTro;
                        old_obj.Update_at = null;
                        old_obj.Create_at = DateTime.Now;
                        old_obj.isDelete = false;
                        old_obj.Delete_at = null;

                        db.SubmitChanges();
                        rs.ErrCode = EnumErrCode.Existent;
                        rs.ErrDesc = "thành công";
                        


                    }
                    else
                    {
                        rs.ErrCode = EnumErrCode.Error;
                        rs.ErrDesc = "Thêm mới thất bại do đã tồn tại Email = " + Email;
                        rs.Data = null;
                    }

                }

            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDesc = "Có lỗi xảy ra trong quá trình thêm mới. Chi tiết lỗi: " + ex.Message;

            }

            return rs;
        }



        public FunctResult<ThongTinTaiKhoan> Sua(int TaiKhoanID ,string HoTen, string DiaChi, string Email, string SoDienThoai, string MatKhau, string VaiTro)
        {
            FunctResult<ThongTinTaiKhoan> rs = new FunctResult<ThongTinTaiKhoan>();

            try
            {
                //cố gắng lấy ra lớp quản lý có mã lớp là maLopQL
                var qr = db.tbl_TaiKhoans.Where(o => o.TaiKhoanID == TaiKhoanID && (o.isDelete == null || o.isDelete == false));

                if (qr.Any())
                {
                    //trường hợp lấy ra được dữ liệu lớp quản lý cần sửa
                    tbl_TaiKhoan old_obj = qr.SingleOrDefault();

                    old_obj.HoTen = HoTen ?? old_obj.HoTen;
                    old_obj.Email = Email;
                    old_obj.SoDienThoai = SoDienThoai;
                    old_obj.MatKhau = MatKhau ?? old_obj.MatKhau;
                    old_obj.DiaChi = DiaChi ?? old_obj.DiaChi;
                    old_obj.VaiTro = VaiTro;
                    old_obj.Update_at = DateTime.Now;


                    db.SubmitChanges();

                    rs.Data = new ThongTinTaiKhoan
                    {
                        HoTen = old_obj.HoTen,
                        Email = old_obj.Email,
                        SoDienThoai = old_obj.SoDienThoai,
                        VaiTro = old_obj.VaiTro,
                        DiaChi = old_obj.DiaChi,
                        MaTK = old_obj.MaTK,
                        TaiKhoanID = old_obj.TaiKhoanID
                    };
                    rs.ErrCode = EnumErrCode.Success;
                    rs.ErrDesc = "Chỉnh sửa thông tin thành công";
                }
                else
                {
                    //trường hợp không tìm thấy lớp quản lý cần sửa

                    rs.ErrCode = EnumErrCode.NotExist;
                    rs.ErrDesc = "Không tìm thấy tài khoản cần sửa";
                }

            }
            catch (Exception ex)
            {
                //nếu lấy ds lớp quản lý lỗi thì trả ra fail
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDesc = "Có lỗi xảy ra trong quá trình chỉnh sửa dữ liệu. Chi tiết lỗi: " + ex.Message;

            }

            return rs;
        }




        public TaiKhoanViewModel LayThongTinViewSua(int id)
        {
            var taiKhoan = db.tbl_TaiKhoans.FirstOrDefault(x => x.TaiKhoanID == id);
            if (taiKhoan == null)
            {
                return null;
            }

            var list = db.tbl_VaiTros.Where(x => x.isDelete == null || x.isDelete == false).ToList();

            return new TaiKhoanViewModel
            {
                taikhoan = taiKhoan,
                DSVT = list
            };
        }


        public FunctResult<tbl_TaiKhoan> Xoa(string id)
        {
            FunctResult<tbl_TaiKhoan> rs = new FunctResult<tbl_TaiKhoan>();
            try
            {
                //cố gắng lấy ra tài khoản có email là 
                var qr = db.tbl_TaiKhoans.Where(o => o.TaiKhoanID == int.Parse(id) && (o.isDelete == null || o.isDelete == false));
                if (qr.Any())
                {
                    tbl_TaiKhoan del_obj = qr.SingleOrDefault();
                    del_obj.isDelete = true;
                    del_obj.Delete_at = DateTime.Now;
                    db.SubmitChanges();
                    rs.ErrCode = EnumErrCode.Success;
                    rs.ErrDesc = "Xóa thành công";

                }
                else
                {
                    rs.ErrCode = EnumErrCode.NotExist;
                    rs.ErrDesc = "không tìm tài khoản cần xóa";

                }
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDesc = "có lỗi trong quá trình xóa";

            }
            return rs;
        }


        public List<tbl_TaiKhoan> Search(string query)
        {
            var list = db.tbl_TaiKhoans.Where(x =>x.VaiTro=="customer" &&(x.isDelete == null || x.isDelete == false));

            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(x => x.HoTen.Contains(query) || x.Email.Contains(query)|| x.SoDienThoai.Contains(query));
            }

            return list.ToList();


        }

        public FunctResult<ThongTinTaiKhoan> Login_action(string email, string password)
        {
            FunctResult<ThongTinTaiKhoan> rs = new FunctResult<ThongTinTaiKhoan>();
            try
            {
                if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(password))
                {
                    rs.ErrCode = EnumErrCode.Empty;
                    rs.ErrDesc = "không được để trống tài khoản mật khẩu";
                }
                else
                {
                    var qr = db.tbl_TaiKhoans.Where(o => o.Email == email && o.MatKhau == password && o.VaiTro == "admin" && (o.isDelete == null || o.isDelete == false));
                    if (qr.Any())
                    {
                        tbl_TaiKhoan tk = qr.SingleOrDefault();

                        rs.Data = new ThongTinTaiKhoan()
                        {
                            TaiKhoanID = tk.TaiKhoanID,
                            MaTK = tk.MaTK,
                            HoTen = tk.HoTen,
                            Email = tk.Email,
                            VaiTro = tk.VaiTro,
                            SoDienThoai = tk.SoDienThoai,
                            DiaChi = tk.DiaChi
                        };

                        rs.ErrCode = EnumErrCode.Success;
                        rs.ErrDesc = "Đăng nhập thành công";
                    }
                    else
                    {
                        rs.Data = null;
                        rs.ErrCode = EnumErrCode.Error;
                        rs.ErrDesc = "Tài khoản hoặc mật khẩu không chính xác";

                    }
                }
            }
            catch (Exception ex) 
            {
                rs.ErrCode=EnumErrCode.Error;
                rs.ErrDesc="Có lôi xảy ra trong quá trình đăng nhập " + ex.Message;

            }
            return rs;
        }

       
    }
}