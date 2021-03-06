using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
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
            smtp.UseDefaultCredentials = false;
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

            try
            {
                smtp.Send(mailMessage);
            }
            catch (Exception e)
            {
                throw new Exception("Error while sending email to user. EmailTo: " + EmailTo + "; Error:\n " + e.ToString());
            }
        }

        public static void SendHtmlEmailTo(string EmailTo, string messageSubject, string htmlBody)
        {
            SMTPInitializer();
            to = new MailAddress(EmailTo);
            MailMessage mailMessage = new MailMessage(from, to);
            mailMessage.IsBodyHtml = true;

            mailMessage.Subject = messageSubject;
            mailMessage.AlternateViews.Add(CreateHtmlForMessage(htmlBody));

            try
            {
                smtp.Send(mailMessage);
            }
            catch (Exception e)
            {
                throw new Exception("Error while sending email to user. EmailTo: " + EmailTo + "; Error: " + e.Message);
            }
        }

        public static void SendHtmlEmailTo(string EmailTo, string messageSubject, string htmlBody, string imagePath, string imageID)
        {
            SMTPInitializer();
            to = new MailAddress(EmailTo);
            MailMessage mailMessage = new MailMessage(from, to);
            mailMessage.IsBodyHtml = true;

            mailMessage.Subject = messageSubject;
            try
            {
                mailMessage.AlternateViews.Add(CreateHtmlForMessage(htmlBody, imagePath, imageID));
                smtp.Send(mailMessage);
            }
            catch (Exception e)
            {
                throw new Exception("Error while sending or creating email to user. EmailTo: " + EmailTo + "; Error: " + e.Message);
            }
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
                    theEmailImage.ContentType.MediaType = MediaTypeNames.Image.Jpeg;
                    theEmailImage.TransferEncoding = TransferEncoding.Base64;
                    htmlView.LinkedResources.Add(theEmailImage);
                }
                catch (Exception e)
                {
                    throw new Exception("Error while creating html to send. Error: " + e.Message);
                }
            }

            return htmlView;
        }
    }
}