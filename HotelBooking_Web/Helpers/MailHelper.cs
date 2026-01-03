using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HotelBooking_Web.Helpers
{
    public class MailHelper
    {
        public static async Task<bool> SendMailAsync(string toEmail, string subject, string content)
        {
            // Lấy thông tin từ Web.config - Máy nào cũng dùng chung bộ này
            var fromEmail = ConfigurationManager.AppSettings["EmailService"];
            var fromPass = ConfigurationManager.AppSettings["EmailPassword"];

            var message = new MailMessage();
            message.From = new MailAddress(fromEmail, "4Anhem Hotel");
            message.To.Add(new MailAddress(toEmail));
            message.Subject = subject;
            message.Body = content;
            message.IsBodyHtml = true;

            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(fromEmail, fromPass);

                try
                {
                    await client.SendMailAsync(message);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}