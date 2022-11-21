using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sendmail_api.Models
{
    public class MailModel
    {
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Body{ get; set; }
    }
}