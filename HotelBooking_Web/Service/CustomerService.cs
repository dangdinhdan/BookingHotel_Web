using HotelBooking_Web.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HotelBooking_Web.Services
{
    public class CustomerService
    {
        private readonly HotelDbContext _db;
        public CustomerService()
        {
            _db = new HotelDbContext();
        }
        public bool IsEmailExist(string email)
        {
            return _db.TaiKhoans.Any(c => c.Email == email);
        }
        public string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        public void RegisterCustomer(TaiKhoanModel customer, string password) 
        {
            customer.MatKhau = HashPassword(password);
            customer.ConfirmPassword = customer.MatKhau;
            customer.VaiTro = "customer";
            customer.Create_at = DateTime.Now;
            customer.isDelete = false;

            if (string.IsNullOrEmpty(customer.SoDienThoai))
            {
                customer.SoDienThoai = "0000000000";
            }

            if (string.IsNullOrEmpty(customer.DiaChi))
            {
                customer.DiaChi = "Chưa cập nhật";
            }

            _db.TaiKhoans.Add(customer);
            _db.SaveChanges();
        }

        public TaiKhoanModel GetCustomerByEmail(string email)
        {
            return _db.TaiKhoans.FirstOrDefault(u => u.Email == email);
        }

        public void UpdateProfile(string email, EditProfileViewModel model)
        {
            var user = _db.TaiKhoans.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.HoTen = model.TenNguoiDung;     
                user.SoDienThoai = model.SoDienThoai;
                user.DiaChi = model.Address;         

                user.Update_at = DateTime.Now;

                user.ConfirmPassword = user.MatKhau;

                _db.SaveChanges(); 
            }
        }
        public bool ChangePassword(string email, string oldPassword, string newPassword, out string error)
        {
            error = "";
            var user = _db.TaiKhoans.FirstOrDefault(u => u.Email == email);
            if (user == null) return false;

            // 1. Kiểm tra mật khẩu cũ (Phải Hash xong mới so sánh được)
            string hashedOld = HashPassword(oldPassword);
            if (user.MatKhau != hashedOld)
            {
                error = "Mật khẩu hiện tại không đúng.";
                return false;
            }

            user.MatKhau = HashPassword(newPassword);

            user.ConfirmPassword = user.MatKhau;

            user.Update_at = DateTime.Now;

            _db.SaveChanges();
            return true;
        }
        public void DeleteAccount(string email)
        {
            var user = _db.TaiKhoans.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.isDelete = true;
                user.Delete_at = DateTime.Now;

                user.ConfirmPassword = user.MatKhau;

                _db.SaveChanges();
            }
        }
        public void RestoreCustomer(TaiKhoanModel newInfo, string password)
        {
            var user = _db.TaiKhoans.FirstOrDefault(u => u.Email == newInfo.Email);

            if (user != null)
            {
                user.HoTen = newInfo.HoTen;
                user.SoDienThoai = newInfo.SoDienThoai;
                user.DiaChi = newInfo.DiaChi ?? "Chưa cập nhật";

                user.MatKhau = HashPassword(password);
                user.ConfirmPassword = user.MatKhau; 

                user.isDelete = false;
                user.Delete_at = null;
                user.Update_at = DateTime.Now;

                _db.SaveChanges();
            }
        }
        public TaiKhoanModel GetAccountIncludeDeleted(string email)
        {
            return _db.TaiKhoans.FirstOrDefault(u => u.Email == email);
        }


    }
}