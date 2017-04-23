using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Configuration;



namespace Resume_Bot.Internal

{

    public static class EmailSender

    {

        private static string apiKey = ConfigurationManager.AppSettings["SendGridKey"];

        public static async Task<bool> SendEmail(string recipient, string sender, string subject, string body)

        {

            try

            {

                dynamic sg = new SendGridClient(apiKey);

                var from = new EmailAddress(sender);

                var to = new EmailAddress(recipient);

                var htmlContent = body;

                var mail = MailHelper.CreateSingleEmail(from, to, subject, body, htmlContent);


                dynamic response = await sg.SendEmailAsync(mail);

                return true;

            }

            catch (Exception)

            {

                return false;

            }

        }

    }

}