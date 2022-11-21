using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using MailKit.Net.Smtp;
using MimeKit;
using System.IO;


namespace sendmail_api
{
    public class EmailService
    {
        public MailerSettings mailSettings; //Pulled from appsettings.json 
        public static string savePath = @"C:\temp\eMailAttempts.txt"; 
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
            if (!IsValidEmail(recipient)) //If the specified recipient isn't a valid eMail address exit process
            {
                logMail(0, recipient, false, "Invalid Recipient Address");
                return;
            }
            MimeMessage email = buildEMailMessage(recipient, subject, body); 
            SmtpClient mailConnection = new SmtpClient();
            mailConnection.Connect(mailSettings.SmtpHost, mailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            while (!isSent && count < ATTEMPT_LIMIT)
            {
                try
                {       
                    mailConnection.Authenticate(mailSettings.SenderAddress, mailSettings.SmtpPass);
                    mailConnection.Send(email);
                    isSent = true;
                    Console.WriteLine("Success!");
                }
                catch (Exception ex)
                {
                    count++;
                    errorToLog = ex.ToString();
                    errorToLog = errorToLog.Substring(0, errorToLog.IndexOf(Environment.NewLine)); //Only return the first line of exception
                }
            }
            logMail(count, recipient,isSent, errorToLog);
            
            
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
