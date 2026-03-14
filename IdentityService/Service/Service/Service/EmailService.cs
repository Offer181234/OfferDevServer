using Interface.Interface;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Service.Service
{
    public class EmailService : IEmailService
    {
        public async Task SendEmail(string toEmail, string subject, string message)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(
                    "ab3041364@gmail.com",
                    "yijktftkaluqtltx"
                ),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("ab3041364@gmail.com"), // Sender Email
                Subject = subject,
                Body = message,
                IsBodyHtml = false,
            };

            mailMessage.To.Add(toEmail); // Receiver Email

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}