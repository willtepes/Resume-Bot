using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Resume_Bot.Dialogs
{
    [Serializable]
    public class ContactForm
    {
        [Prompt(new string[] { "What is your name?" })]
        public string Name { get; set; }

        [Prompt("How can Will contact you? Please enter your email address")]
        public string Contact { get; set; }

        [Prompt("What's your message?")]
        public string Message { get; set; }

        public static IForm<ContactForm> BuildForm()
        {
            return new FormBuilder<ContactForm>()
                .Field(nameof(Contact), validate: ValidateContactInformation)
                .Field(nameof(Message), active: ContactEnabled)
                .AddRemainingFields()
                .Build();
        }

        private static bool ContactEnabled(ContactForm state) =>
            !string.IsNullOrWhiteSpace(state.Contact) && !string.IsNullOrWhiteSpace(state.Name);

        private static Task<ValidateResult> ValidateContactInformation(ContactForm state, object response)
        {
            var result = new ValidateResult();
            string contactInfo = string.Empty;
            if (GetEmailAddress((string)response, out contactInfo))
            {
                result.IsValid = true;
                result.Value = contactInfo;
            }
            else
            {
                result.IsValid = false;
                result.Feedback = "You did not enter a valid email address.";
            }
            return Task.FromResult(result);
        }

        private static bool GetEmailAddress(string response, out string contactInfo)
        {
            contactInfo = string.Empty;
            var match = Regex.Match(response, @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
            if (match.Success)
            {
                contactInfo = match.Value;
                return true;
            }
            return false;
        }

    }
}