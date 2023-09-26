using System.Net;
using System.Net.Mail;

namespace ERPSEI.Email
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            string mail = "omeollincozcacuauhtli@gmail.com";
            string password = "pccnvttjxauanieo";

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            MailMessage msg = new MailMessage(
                        from: mail,
                        to: email,
                        subject,
                        message
                    );

            msg.IsBodyHtml = true;

            return client.SendMailAsync(msg);
                
        }
    }
}
