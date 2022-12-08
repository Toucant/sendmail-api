using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using sendmail_api.Models;

namespace sendmail_api.DataAccess
{
    public class MailContext : DbContext
    {
        public MailContext() : base("MailContext")
        { }
        public DbSet<LogModel> LogList{ get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}