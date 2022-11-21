using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using MailKit.Net.Smtp;
using MimeKit;
using System.IO;
using System.Text.Json;

namespace TestInConsole
{
    public class EmailService
    {
        public MailerSettings mailSettings; //Pulled from appsettings.json 
        public static string savePath = @"C:\temp\eMailAttempts.txt"; //Considered adding to appsettings.json but just left this here
        private readonly int ATTEMPT_LIMIT = 3; //Amount of tries before giving up and logging the record
        public EmailService(MailerSettings mailerSettings)
        {
            mailSettings = mailerSettings;
        }
        public void SendEmail(string recipient, string subject, string body) //take in credentials?
        {
            int count = 0;
            bool isSent = false;
            string errorToLog = "";
            SmtpClient client = new SmtpClient();
            if (!IsValidEmail(recipient)) //If the specified recipient isn't valid then exit process now
            {
                logMail(0, recipient, "Invalid Recipient Address");
                return;
            }
            MimeMessage email = buildEMailMessage(recipient, subject, body); 
            using SmtpClient mailConnection = new SmtpClient();
            while (!isSent && count <= ATTEMPT_LIMIT)
            {
                try
                {
                    mailConnection.Connect(mailSettings.SmtpHost, mailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls); //TLS
                    mailConnection.Authenticate(mailSettings.SenderAddress, mailSettings.SmtpPass);
                    mailConnection.Send(email);
                    isSent = true;
                }
                catch (Exception ex)
                {
                    count++;
                    errorToLog = ex.ToString();
                }
            }
            if (errorToLog != "" && count >= ATTEMPT_LIMIT) 
            {
                logMail(count, recipient, errorToLog);
            }
            
            mailConnection.Disconnect(true);
        }

        private MimeMessage buildEMailMessage(string recipient, string subject, string body)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(mailSettings.SenderAddress));
            message.To.Add(MailboxAddress.Parse(recipient));
            message.Subject = subject;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = body };
            return message;
        }
        private static void logMail(int attempts, string emailAddress, string logNote)
        {
            string textBuild = emailAddress + ", \t" + attempts + ", \t" + DateTime.Now + ", \t" + logNote;
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
    }
   
}
