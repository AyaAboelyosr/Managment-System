using Managment_System.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.EF.Repositories
{
   public class EmailJob :IEmailJob
    {
        private readonly IEmailService _mailService;

        public EmailJob(IEmailService mailService)
        {
            _mailService = mailService;
        }

        public async Task SendScheduledEmail(string toEmail, string subject)
        {
            try
            {
                await _mailService.sendEmailAsync(toEmail, subject);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

    }
}
