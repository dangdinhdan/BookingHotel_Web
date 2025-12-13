using HotelBooking_Web.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HotelBooking_Web.Services
{
    public class CustomerService
    {
        private readonly DataClasses1DataContext db;

        public CustomerService()
        {
            db = new DataClasses1DataContext();
        }

        public bool IsEmailExist(string email)
        {
            return db.tbl_TaiKhoans.Any(c => c.Email == email);
        }

        public string HashPassword(string password)
        {
            //using (SHA256 sha = SHA256.Create())
            //{
            //    var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            //    return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            //}
            return password;
        }

        public void RegisterCustomer(TaiKhoanModel model, string rawPassword)
        {
            var newAccount = new tbl_TaiKhoan
            {
                HoTen = model.HoTen,
                Email = model.Email,
                SoDienThoai = string.IsNullOrEmpty(model.SoDienThoai) ? "0000000000" : model.SoDienThoai,
                DiaChi = string.IsNullOrEmpty(model.DiaChi) ? "Chưa cập nhật" : model.DiaChi,
                //MatKhau = HashPassword(rawPassword),
                MatKhau = rawPassword,
                VaiTro = "customer",
                Create_at = DateTime.Now,
                isDelete = false
            };

            db.tbl_TaiKhoans.InsertOnSubmit(newAccount);
            db.SubmitChanges();
        }

        public tbl_TaiKhoan GetAccount(string email)
        {
            return db.tbl_TaiKhoans.FirstOrDefault(u => u.Email == email && u.isDelete == false);
        }

        public tbl_TaiKhoan GetAccountIncludeDeleted(string email)
        {
            return db.tbl_TaiKhoans.FirstOrDefault(u => u.Email == email);
        }

        public void UpdateProfile(string email, EditProfileViewModel model)
        {
            var user = db.tbl_TaiKhoans.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.HoTen = model.TenNguoiDung;
                user.SoDienThoai = model.SoDienThoai;
                user.DiaChi = model.Address;
                user.Update_at = DateTime.Now;

                db.SubmitChanges();
            }
        }

        public void RestoreCustomer(TaiKhoanModel newInfo, string password)
        {
            var user = db.tbl_TaiKhoans.FirstOrDefault(u => u.Email == newInfo.Email);

            if (user != null)
            {
                user.HoTen = newInfo.HoTen;
                user.SoDienThoai = newInfo.SoDienThoai;
                user.DiaChi = newInfo.DiaChi ?? "Chưa cập nhật";
                //user.MatKhau = HashPassword(password);
                user.MatKhau = password;

                user.isDelete = false;
                user.Delete_at = null;
                user.Update_at = DateTime.Now;

                db.SubmitChanges();
            }
        }

        public bool ChangePassword(string email, string oldPassword, string newPassword, out string error)
        {
            error = "";
            var user = db.tbl_TaiKhoans.FirstOrDefault(u => u.Email == email && u.isDelete == false);

            if (user == null)
            {
                error = "Tài khoản không tồn tại.";
                return false;
            }

            //string oldPassHash = HashPassword(oldPassword);
            //if (user.MatKhau != oldPassHash)
            //{ ... }

            if (user.MatKhau != oldPassword)
            {
                error = "Mật khẩu cũ không chính xác.";
                return false;
            }

            // user.MatKhau = HashPassword(newPassword);

            user.MatKhau = newPassword;

            user.Update_at = DateTime.Now;

            try
            {
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = "Lỗi: " + ex.Message;
                return false;
            }
        }

        public void DeleteAccount(string email)
        {
            var user = db.tbl_TaiKhoans.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.isDelete = true;
                user.Delete_at = DateTime.Now;

                db.SubmitChanges();
            }
        }

        

        internal object GetCustomerByEmail(string email)
        {
            throw new NotImplementedException();
        }


        public bool ResetPassword(string email, string phone, string newPassword, out string error)
        {
            error = "";

            var user = db.tbl_TaiKhoans.FirstOrDefault(u => u.Email == email && u.SoDienThoai == phone && u.isDelete == false);

            if (user == null)
            {
                error = "Thông tin không chính xác. Vui lòng kiểm tra lại Email và Số điện thoại.";
                return false;
            }

            user.MatKhau = newPassword;
            user.Update_at = DateTime.Now;

            try
            {
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = "Lỗi hệ thống: " + ex.Message;
                return false;
            }
        }
    }

}