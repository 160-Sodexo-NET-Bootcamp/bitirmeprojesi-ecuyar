using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace BackgroundWorker.Core
{
    public class WelcomeEmailSender
    {
        private string To { get; set; }
        private object DynamicData { get; set; }

        public WelcomeEmailSender(string To, object DynamicData)
        {
            this.To = To;
            this.DynamicData = DynamicData;
        }

        public Task<Response> Main()
        {
            return Execute();
        }

        async Task<Response> Execute()
        {
            //var apiKey = Environment.GetEnvironmentVariable("SendGridKey");
            //key normally stored in the EnvironmentVariables but it is hardcoded for this project
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            //sendgrid validated sender
            var from = new EmailAddress("mylittleshop.120@gmail.com", "My Little Shop");
            var to = new EmailAddress(To);

            //there is a template that I created in the website. This is its id.
            var templateId = "d-b6821d1cd6394156926fa07a66a80c79";
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, DynamicData);
            var result = await client.SendEmailAsync(msg);

            return result;
        }
    }
}
