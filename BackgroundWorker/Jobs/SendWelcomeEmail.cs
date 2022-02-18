using Core.EmailService;
using Hangfire;

namespace BackgroundWorker.Jobs
{
    public class SendWelcomeEmail
    {
        public SendWelcomeEmail()
        {
        }

        [AutomaticRetry(Attempts = 5, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public void WelcomeEmailSendJob(WelcomeEmailSender WelcomeEmailSender)
        {
            //method that sends email
            WelcomeEmailSender.Main();
        }

    }
}
