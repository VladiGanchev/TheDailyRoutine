using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TheDailyRoutine.Common.Models;

namespace TheDailyRoutine.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var emailConfig = configuration.GetSection("EmailSettings");

            if (!emailConfig.Exists())
            {
                throw new InvalidOperationException("EmailSettings section is missing from configuration");
            }

            services.Configure<EmailSettings>(emailConfig);

            // Verify configuration is loaded correctly
            var emailSettings = new EmailSettings();
            emailConfig.Bind(emailSettings);

            var logger = services.BuildServiceProvider()
                .GetService<ILogger<EmailSettings>>();

            logger?.LogInformation("Loaded Email Settings: {@Settings}", new
            {
                Server = emailSettings.SmtpServer,
                Port = emailSettings.SmtpPort,
                Username = emailSettings.SmtpUsername,
                FromEmail = emailSettings.FromEmail,
                FromName = emailSettings.FromName
            });

            if (string.IsNullOrEmpty(emailSettings.SmtpServer))
            {
                throw new InvalidOperationException("SMTP Server is not configured in appsettings.json");
            }

            return services;
        }
    }
}