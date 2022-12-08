using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace sendmail_api.Models
{
    public class LogModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public int AttemptCount { get; set; }
        public string PassFail { get; set; }
        public DateTime DateTried { get; set; }
        public string LogNote { get; set; }
    }
}