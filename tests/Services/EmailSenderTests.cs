using CompanyPMO_.NET.Services;
using FluentAssertions;
using netDumbster.smtp;
using FakeItEasy;

namespace Tests.Services
{
    public class EmailSenderTests()
    {
        [Fact]
        public async void SendEmailAsync_SendEmailToNetDumbster()
        {
            var port = 9009;
            using var server = SimpleSmtpServer.Start(port);

            var emailSender = new EmailSender("localhost", port, false);

            // Pass actuals creds to actually send a test email
            Environment.SetEnvironmentVariable("EMAIL", "your_test_email@example.com");
            Environment.SetEnvironmentVariable("EMAIL_PASSWORD", "your_test_password");

            var fakeEmailSender = A.Fake<IEmailSender>();
            A.CallTo(() => fakeEmailSender.LoadEnv()).DoesNothing();

            await emailSender.SendEmailAsync("test@example.com", "Test Subject", "Test Body");

            server.ReceivedEmail.Should().HaveCount(1);
        }
    }
}
