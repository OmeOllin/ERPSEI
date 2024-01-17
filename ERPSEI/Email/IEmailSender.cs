namespace ERPSEI.Email
{
    public interface IEmailSender
    {
        void SendEmailAsync(string email, string subject, string message);
    }
}
