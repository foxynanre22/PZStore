using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace PZStore.SyntaticSugar
{
    public static class EmailSender
    {
        private static MailAddress from = new MailAddress("pzstore9@gmail.com", "Registration");
        private static MailAddress to = null;
        private static SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");

        private static void SMTPInitializer()
        {
            smtp.Port = 587;
            smtp.UseDefaultCredentials = true;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential("pzstore9@gmail.com", "lfyybk546J@");
        }

        public static void SendEmailTo(string EmailTo, string messageSubject, string messageText, bool isHTML)
        {
            SMTPInitializer();
            to = new MailAddress(EmailTo);
            MailMessage mailMessage = new MailMessage(from, to);
            if (isHTML) { mailMessage.IsBodyHtml = true; }

            mailMessage.Subject = messageSubject;
            mailMessage.Body = messageText;

            smtp.Send(mailMessage);
        }

        public static void SendHtmlEmailTo(string EmailTo, string messageSubject, string htmlBody)
        {
            SMTPInitializer();
            to = new MailAddress(EmailTo);
            MailMessage mailMessage = new MailMessage(from, to);
            mailMessage.IsBodyHtml = true;

            mailMessage.Subject = messageSubject;
            mailMessage.AlternateViews.Add(CreateHtmlForMessage(htmlBody));

            smtp.Send(mailMessage);

        }

        public static void SendHtmlEmailTo(string EmailTo, string messageSubject, string htmlBody, string imagePath, string imageID)
        {
            SMTPInitializer();
            to = new MailAddress(EmailTo);
            MailMessage mailMessage = new MailMessage(from, to);
            mailMessage.IsBodyHtml = true;

            mailMessage.Subject = messageSubject;
            mailMessage.AlternateViews.Add(CreateHtmlForMessage(htmlBody, imagePath, imageID));

            smtp.Send(mailMessage);
        }

        private static AlternateView CreateHtmlForMessage(string htmlBody, string imagePath = null, string imageID = null)
        {
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");

            if(imagePath != null && imageID != null)
            {
                try
                {
                    LinkedResource theEmailImage = new LinkedResource(imagePath);
                    theEmailImage.ContentId = imageID;
                    htmlView.LinkedResources.Add(theEmailImage);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            return htmlView;
        }
    }
}