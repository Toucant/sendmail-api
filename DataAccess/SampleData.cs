using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using sendmail_api.Models;
namespace sendmail_api.DataAccess
{
    public class SampleData : System.Data.Entity.DropCreateDatabaseIfModelChanges<MailContext>
    {
        protected override void Seed(MailContext context)
        {
            var MailLogs = new List<LogModel>
            {
                new LogModel{EmailAddress="test@test.com",AttemptCount=3, PassFail="FAIL", DateTried = DateTime.Parse("2022-12-5")},
                new LogModel{EmailAddress="mail@test.com",AttemptCount=3, PassFail="FAIL", DateTried = DateTime.Parse("2022-12-5")},
                new LogModel{EmailAddress="oldtest@mail.com",AttemptCount=2, PassFail="SUCCESS", DateTried = DateTime.Parse("2022-11-5")},
            };
            MailLogs.ForEach(l => context.LogList.Add(l));
            context.SaveChanges();
        }
    }
    
}