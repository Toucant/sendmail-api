using Microsoft.Extensions.Configuration;
using sendmail_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace sendmail_api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Index(MailModel mailInfo) //string subject, string body
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
              .AddJsonFile("appsettings.json").Build();
            var section = config.GetSection(nameof(MailerSettings));
            var mailSettingsConfig = section.Get<MailerSettings>();
            EmailService mailService = new EmailService(mailSettingsConfig);
            mailService.SendEmail(mailInfo.ToAddress, mailInfo.Subject, mailInfo.Body);
            Console.WriteLine();
            return View();

        }
    }
}