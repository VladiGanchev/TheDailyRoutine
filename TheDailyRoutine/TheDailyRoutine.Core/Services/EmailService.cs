using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using TheDailyRoutine.Core.Contracts;
using TheDailyRoutine.Common.Models;
using TheDailyRoutine.Core.Models.ServiceModels.Notifications; 

namespace TheDailyRoutine.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IOptions<EmailSettings> emailSettings,
            ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;

            _logger.LogInformation("Email Settings: {@Settings}", new
            {
                Server = _emailSettings.SmtpServer,
                Port = _emailSettings.SmtpPort,
                Username = _emailSettings.SmtpUsername,
                FromEmail = _emailSettings.FromEmail,
                FromName = _emailSettings.FromName
            });
        }

        public async Task<bool> SendNotificationEmailAsync(string userEmail, string subject, string content)
        {
            try
            {
                using var smtpClient = new SmtpClient
                {
                    Host = _emailSettings.SmtpServer,
                    Port = _emailSettings.SmtpPort,
                    EnableSsl = _emailSettings.UseSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(
                        _emailSettings.SmtpUsername,
                        _emailSettings.SmtpPassword)
                };

                using var message = new MailMessage
                {
                    From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                    Subject = subject,
                    Body = content,
                    IsBodyHtml = true
                };

                message.To.Add(userEmail);

                await smtpClient.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {UserEmail}", userEmail);
                return false;
            }
        }

        public Task<bool> SendReminderEmailAsync(string userEmail, IEnumerable<string> incompleteHabits)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendStreakMilestoneEmailAsync(string userEmail, string habitTitle, int streakCount)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendWeeklySummaryEmailAsync(string userEmail, WeeklySummaryNotificationModel summary)
        {
            throw new NotImplementedException();
        }
    }
}