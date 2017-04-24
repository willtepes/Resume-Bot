using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Luis;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Luis.Models;
using System.Threading;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.FormFlow;
using System.Text;
using System.Configuration;
using Resume_Bot.Internal;



namespace Resume_Bot.Dialogs

{
    [LuisModel("bb95a81d-43e8-4140-b438-eddc943b2aab", "397fb5d8ce894d51b945ef70b6da32d3")]
    [Serializable]
    public class Resume_BotLuisDialog : LuisDialog<object>
    {
        public Resume_BotLuisDialog(params ILuisService[] services) : base(services)
        {
        }

        [LuisIntent("None")]
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry. I didn't understand you. Could you rephrase that?");
            context.Wait(MessageReceived);
        }

        [LuisIntent("contact")]
        public async Task contact(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("Thank you for reaching out. You will need to provide few details about yourself before entering your comment.");
                var contactForm = new FormDialog<ContactForm>(new ContactForm(), ContactForm.BuildForm, FormOptions.PromptInStart);
                context.Call(contactForm, ContactFormComplete);
            }
            catch (Exception)
            {
                await context.PostAsync("An error was encountered. You can try again later and meanwhile I'll check what went wrong.");
                context.Wait(MessageReceived);
            }

        }

        [LuisIntent("education")]
        public async Task education(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(@"I have completed the following educational studies.");
            await context.PostAsync(@"FULL STACK WEB DEV CERTIFICATION • 2017 • CODING DOJO");
            await context.PostAsync(@"BA IN POLITICAL SCIENCE• 2001 • UNIVERSITY OF WASHINGTON");
            context.Wait(MessageReceived);
        }

        [LuisIntent("experience")]
        public async Task experience(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I have completed an intensive web development program with more than 1000 hours spent learning and building 50+ single and multi-page web apps using Python, JavaScript, and C#.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("objective")]
        public async Task objective(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I am an experienced IT support professional now pursuing a career in software development. I am seeking a web developer position where I can bring my passion for building cool stuff and continue to expand my skill set.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("skills")]
        public async Task skills(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(@"Here are a few of the technologies I am familiar with");
            await context.PostAsync(@"Languages: Python ▪ JavaScript ▪ C# ▪ HTML/CSS");
            await context.PostAsync(@"Frameworks: Express.js ▪ AngularJS ▪ ASP.NET Core ▪ Entity Framework Core ▪ Django ▪ NancyFX ▪ Flask");
            await context.PostAsync(@"Database: MySQL ▪ MongoDB ▪ Dapper ORM ▪ SQLite");
            await context.PostAsync(@"Other: ▪ jQuery ▪ Git ▪ GitHub ▪ Bower ▪ Node.js ▪ React");
            context.Wait(MessageReceived);
        }

        [LuisIntent("help")]
        public async Task help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(Responses.HelpMessage);
            context.Wait(MessageReceived);
        }

        #region Private 
        private static string recipientEmail = ConfigurationManager.AppSettings["RecipientEmail"];
        private static string senderEmail = ConfigurationManager.AppSettings["SenderEmail"];

        private async Task ContactFormComplete(IDialogContext context, IAwaitable<ContactForm> result)
        {
            try
            {
                var contact = await result;
                string message = GenerateEmailMessage(contact);
                var success = await EmailSender.SendEmail(recipientEmail, senderEmail, $"Email from {contact.Name}", message);
                if (!success)
                    await context.PostAsync("I was not able to send your message. Something went wrong.");
                else
                {
                    await context.PostAsync("Thanks for the message.");
                    await context.PostAsync("What else would you like to do?");
                }

            }
            catch (FormCanceledException)
            {
                await context.PostAsync("Don't want to send a message? No problem.");
            }
            catch (Exception)
            {
                await context.PostAsync("An error was encountered. You can try again later and meanwhile I'll check what went wrong.");
            }
            finally
            {
                context.Wait(MessageReceived);
            }
        }

        private string GenerateEmailMessage(ContactForm contact)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Message from: {contact.Name}");
            sb.AppendLine($"Contact: {contact.Contact}");
            sb.AppendLine($"Message: {contact.Message}");
            return sb.ToString();
        }

        #endregion
    }
}

