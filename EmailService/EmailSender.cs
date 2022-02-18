using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace EmailService
{
    public class EmailSender
    {
        public string subject { get; set; }
        public string to { get; set; }
        public string contentPlain { get; set; }
        public string contentHtml { get; set; }
        public string username { get; set; }
        public object dynamicData { get; set; }

        public EmailSender(string subject, string to, string contentPlain, string contentHtml, string username)
        {
            this.subject = subject;
            this.to = to;
            this.contentPlain = contentPlain;
            this.contentHtml = contentHtml;
            this.username = username;
        }

        public EmailSender(string to, object dynamicData)
        {
            this.to = to;
            this.dynamicData = dynamicData;
        }

        public Task<Response> Main()
        {
            var operationResult = Execute();

            return operationResult;
        }





        async Task<Response> Execute()
        {


            //TODO : key silinecek
            //var apiKey = Environment.GetEnvironmentVariable("SendGridKey");
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("mylittleshop.120@gmail.com", "My Little Shop");
            //var subject = this.subject;
            var subject = "Denemee";
            var to = new EmailAddress(this.to, this.username);
            var plainTextContent = this.contentPlain;
            //var htmlContent = $"<strong>{this.contentHtml}</strong>";
            var htmlContent = this.contentHtml;
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, "d-b6821d1cd6394156926fa07a66a80c79", dynamicData);

            var result = await client.SendEmailAsync(msg);

            return result;
        }
    }
}
