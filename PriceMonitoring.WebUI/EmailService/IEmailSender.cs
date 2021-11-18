using System.Threading.Tasks;

namespace PriceMonitoring.WebUI.EmailService
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
