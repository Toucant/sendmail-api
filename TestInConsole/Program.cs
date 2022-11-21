using Microsoft.Extensions.Configuration;
using System;
using static TestInConsole.EmailService;
namespace TestInConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();
            var section = config.GetSection(nameof(MailerSettings));
            var mailSettingsConfig = section.Get<MailerSettings>();
            EmailService test = new EmailService(mailSettingsConfig);
            
            
            test.SendEmail("jonathon.ferry@ethereal.email", "Test From Console", "Here is the test run. Check log file to see if stored #2");
        }
    }
}
