
using System.Net.Mail;
using System.Net;

namespace KarmaWebMVC.Repository
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("quangvu252002@gmail.com", "gndahgwwiwazrsdm")
            };

            return client.SendMailAsync(
                new MailMessage(from: "quangvu252002@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }

    }
}
