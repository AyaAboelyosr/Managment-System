using Hangfire;
using Managment_System.Core.DTO;
using Managment_System.Core.Interfaces;
using Managment_System.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private readonly IEmailJob emailJob;
        private readonly IUnitOfWork unitOfWork;

        public ReminderController(IEmailJob emailJob, IUnitOfWork unitOfWork)
        {
            this.emailJob = emailJob;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("CreateReminder")]
        [Authorize(Roles = "Admin,Leader,User")]
        public async Task<IActionResult> CreateReminder([FromBody] ReminderDTO reminderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var reminder = new Reminder
                {
                    Title = reminderDTO.Title,
                    ReminderDateTime = reminderDTO.ReminderDateTime
                };

                await unitOfWork.ReminderRepository.AddAsync(reminder);
                unitOfWork.Save();

                var timeSpan = reminder.ReminderDateTime - DateTime.Now;
                var userEmail = User.FindFirstValue(ClaimTypes.Email);

                if (!string.IsNullOrEmpty(userEmail))
                {
                  //  string body = $"This is a reminder for the task: {reminder.Title}.";

                    BackgroundJob.Schedule(() =>
                        emailJob.SendScheduledEmail(userEmail, reminder.Title),
                        timeSpan);
                }

                return Ok(new { Message = "Reminder created successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.InnerException?.Message}");

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
