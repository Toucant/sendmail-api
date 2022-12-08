using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using MailKit.Net.Smtp;
using MimeKit;
using System.IO;
using System.Data.Entity;
using sendmail_api.DataAccess;
using sendmail_api.Models;
//Schema: clarity-email
namespace sendmail_api
{
    public class EmailService
    {
        private MailerSettings mailSettings; //Pulled from appsettings.json 
        private static string savePath = @"C:\temp\eMailAttempts.txt"; //Set savePath before running
        private readonly int ATTEMPT_LIMIT = 3; //Amount of tries before giving up and logging the record
        private MailContext context;
        private bool logToFile = true;
        #region Constructors & Setters 
        public EmailService(MailerSettings mailerSettings)
        {
            mailSettings = mailerSettings;
        }
        public EmailService(MailerSettings mailerSettings,MailContext mailContext, bool logToTextFile)
        {
            mailSettings = mailerSettings;
            context = mailContext;
            logToFile = logToTextFile;
        }

        public void configureMailSettings(MailerSettings mailerSettings)
        {
            mailSettings = mailerSettings;
        }
        public void configureContextSettings(MailContext mailContext)
        {
            context = mailContext;
        }
        public void changeLogType(bool logFile)
        {
            logToFile = logFile;
        }
        #endregion
        #region Primary Fucntions
        public void SendEmail(string recipient, string subject, string body)
        {
            int count = 0;
            bool isSent = false;
            string errorToLog = "";
            if (!IsValidEmail(recipient)) //If the specified recipient isn't a valid eMail address exit process
            {
                logMail(0, recipient, false, "Invalid Recipient Address");
                return;
            }
            MimeMessage email = buildEMailMessage(recipient, subject, body);
            if (email == null)
            {
                return;
            }

            SmtpClient mailConnection = new SmtpClient();

            mailConnection.Connect(mailSettings.SmtpHost, mailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            while (!isSent && count < ATTEMPT_LIMIT)
            {
                try
                {
                    //mailConnection.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    mailConnection.Authenticate(mailSettings.SenderAddress, mailSettings.SmtpPass);
                    mailConnection.Send(email);
                    isSent = true;
                }
                catch (Exception ex)
                {
                    count++;
                    errorToLog = ex.ToString();
                    errorToLog = errorToLog.Substring(0, errorToLog.IndexOf(Environment.NewLine)); //Only return the first line of exception
                }
            }
            if (logToFile)
            {
                logMail(count, recipient, isSent, errorToLog);
            } else
            {
                logMailToServer(count, recipient, isSent, errorToLog);
            }

            mailConnection.Disconnect(true);
        }

        private MimeMessage buildEMailMessage(string recipient, string subject, string body)
        {
            MimeMessage message = new MimeMessage();
            try { 
                message.From.Add(MailboxAddress.Parse(mailSettings.SenderAddress));
            } catch(Exception ex) //Invalid appsetting.json values
            {
                Console.WriteLine("Fill in values in appsettings.json");
                throw ex;
            }
            message.To.Add(MailboxAddress.Parse(recipient));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };
            return message;
        }
        private void logMailToServer(int attempts, string emailAddress, bool status, string logNote)
        {
            var MailLog = new LogModel { EmailAddress = emailAddress,AttemptCount = attempts, PassFail = (status == true ? "SUCCESS" : "FAIL"), DateTried=DateTime.Now,LogNote = logNote };
            context.LogList.Add(MailLog);
            context.SaveChanges();
        }
        private static void logMail(int attempts, string emailAddress, bool status, string logNote)
        {
            string textBuild = emailAddress + ", \t" + attempts + ", \t" + (status == true ? "SUCCESS" : "FAILED") + ", \t" + DateTime.Now +
                (logNote != "" ? ", \t" + logNote : "");
            if (!File.Exists(savePath))
            {       
                using (StreamWriter sw = File.CreateText(savePath))
                {
                    sw.WriteLine(textBuild);
                }
            }
            using (StreamWriter sw = File.AppendText(savePath))
            {
                sw.WriteLine(textBuild);
            }
        }
        #endregion
        #region Misc & Utility Functions
        private static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }

}
