using System.Net;
using System.Net.Mail;

namespace ERPSEI.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly string mailAddress;
        private readonly SmtpClient smtpClient;

        public EmailSender(string _mailAddress, string _mailPassword, string _smtpServer, int _smtpPort) { 
            mailAddress = _mailAddress;
            smtpClient = new SmtpClient(_smtpServer, _smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_mailAddress, _mailPassword)
            };
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            MailMessage msg = new MailMessage(
                        from: mailAddress,
                        to: email,
                        subject,
                        message
                    );

            msg.IsBodyHtml = true;

            return smtpClient.SendMailAsync(msg);
                
        }
    }
}
