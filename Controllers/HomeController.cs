using Microsoft.Extensions.Configuration;
using sendmail_api.DataAccess;
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
        private MailContext db = new MailContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(MailModel mailInfo) //string subject, string body
        {
            //Grab settings from JSON file
            var config = new ConfigurationBuilder()
              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
              .AddJsonFile("appsettings.json").Build();
            //Microsoft.Extensions.Configuration library
            var section = config.GetSection(nameof(MailerSettings));
            var mailSettingsConfig = section.Get<MailerSettings>();

            MailContext context = new MailContext();

            EmailService mailService = new EmailService(mailSettingsConfig,context, true);
            mailService.SendEmail(mailInfo.ToAddress, mailInfo.Subject, mailInfo.Body);
            return View();
        }
        public ViewResult MailLogs()
        {
            return View(db.LogList.ToList());
        }
    }
}