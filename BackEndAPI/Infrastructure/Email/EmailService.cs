using System.Net;
using System.Net.Mail;
using UseCase.Shared.Services;

namespace Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private const string HOST = "smtp.gmail.com";
        private const int PORT = 587;
        private static string _email = UseCase.Configuration.Email;
        private static string _password = UseCase.Configuration.Password;

        public async Task SendAsync(
            string email,
            string title,
            string message,
            CancellationToken cancellationToken = default)
        {
            MailMessage mail = new MailMessage(_email, email)
            {
                Subject = title,
                Body = message,
                IsBodyHtml = true
            };

            using (SmtpClient smtp = new SmtpClient(HOST, PORT))
            {
                smtp.Credentials = new NetworkCredential(_email, _password);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(mail, cancellationToken);
            }
        }
    }
}
