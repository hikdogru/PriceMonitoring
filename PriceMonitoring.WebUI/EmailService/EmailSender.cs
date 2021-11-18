using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PriceMonitoring.WebUI.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailSender(EmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }
        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmail(message: message);
            Send(emailMessage: emailMessage);
        }

        private void Send(MimeMessage emailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(host: _emailConfiguration.SmtpServer, port: _emailConfiguration.Port, useSsl: false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfiguration.UserName, _emailConfiguration.Password);
                client.Send(emailMessage);
            }
            catch
            {
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

        private MimeMessage CreateEmail(Message message)
        {
            string FilePath = Directory.GetCurrentDirectory() + @"\Views\Shared\ConfirmPage.html";
            StreamReader str = new StreamReader(FilePath);
            string mailText = str.ReadToEnd();
            mailText = mailText.Replace("[username]", message.Content.Split(" ")[0]).Replace("[email]", message.Content.Split(" ")[1]);
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfiguration.From));
            emailMessage.To.Add(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            { Text = mailText };
            return emailMessage;
        }


        public async Task SendEmailAsync(Message message)
        {
            var emailMessage = CreateEmail(message: message);
            await SendAsync(emailMessage: emailMessage);
        }

        private async Task SendAsync(MimeMessage emailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(host: _emailConfiguration.SmtpServer, port: _emailConfiguration.Port, useSsl: true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password);
                await client.SendAsync(emailMessage);
            }
            catch
            {
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
