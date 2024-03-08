using dotenv.net;
using System.Net;
using System.Net.Mail;

namespace CompanyPMO_.NET.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        void LoadEnv();
    }
    public class EmailSender : IEmailSender
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly bool _enableSsl;
        public EmailSender(string smtpServer = "smtp.gmail.com", int smtpPort = 587, bool enableSsl = true)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _enableSsl = enableSsl;
        }

        public void LoadEnv()
        {
            DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { "./.env" }));
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            LoadEnv();

            string emailAddress = Environment.GetEnvironmentVariable("EMAIL");
            string password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");

            var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(emailAddress, password),
                EnableSsl = _enableSsl
            };

            return client.SendMailAsync(new MailMessage(from: emailAddress, to: email, subject, message));
        }
    }
}
