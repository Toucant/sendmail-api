using System;
using System.Collections.Generic;
using System.Text;

namespace TestInConsole
{
    public class MailerSettings
    {
        public string SenderAddress { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
    }
}
