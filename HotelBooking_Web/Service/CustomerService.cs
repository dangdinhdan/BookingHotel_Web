using HotelBooking_Web.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;

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
            return _db.Customers.Any(c => c.Email == email);
        }

        public string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        public string GenerateEmailToken()
        {
            return Guid.NewGuid().ToString();
        }

        public void RegisterCustomer(Customer customer, string password)
        {
            customer.PasswordHash = HashPassword(password);
            customer.EmailConfirmationToken = GenerateEmailToken();
            _db.Customers.Add(customer);
            _db.SaveChanges();

            SendConfirmationEmail(customer.Email, customer.EmailConfirmationToken);
        }

        public void SendConfirmationEmail(string email, string token)
        {
            string confirmationLink = $"https://localhost:44300/Customer/ConfirmEmail?token={token}";

            MailMessage message = new MailMessage();
            message.To.Add(email);
            message.Subject = "Xác nhận Email";
            message.Body = $"Nhấn vào link để xác nhận email: {confirmationLink}";
            message.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential("youremail@gmail.com", "password"); // đổi email thật
            smtp.EnableSsl = true;
            smtp.Send(message);
        }

        public bool ConfirmEmail(string token)
        {
            var customer = _db.Customers.FirstOrDefault(c => c.EmailConfirmationToken == token);
            if (customer != null)
            {
                customer.IsEmailConfirmed = true;
                customer.EmailConfirmationToken = null;
                _db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
