using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using MLS_Data.DataModels;
using System.Text;
using System.Threading.Tasks;

namespace MLS_Data.Shared
{
    public class RegisterConfirmation
    {
        private string Email { get; set; }
        private string EmailConfirmationUrl { get; set; }

        private readonly UserManager<ApplicationUser_DataModel> userManager;

        public RegisterConfirmation(UserManager<ApplicationUser_DataModel> userManager, string Email)
        {
            this.userManager = userManager;
            this.Email = Email;
        }

        public async Task<string> OnGetAsync()
        {
            var user = await userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                return "Error";
            }

            var userId = user.Id;
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            EmailConfirmationUrl = $"/confirm/?userId={userId}?token={code}";

            return EmailConfirmationUrl;
        }
    }
}
