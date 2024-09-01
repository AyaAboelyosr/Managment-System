using Managment_System.Core.DTO;
using Managment_System.Core.Interfaces;
using Managment_System.EF.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Managment_System.Core.Models;
using Managment_System.Response;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;


namespace Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileService fileService;
        public TaskController(IUnitOfWork unitOfWork, IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.fileService = fileService;
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Leader")]
        public async Task<IActionResult> CreateTask([FromForm] AddTaskDTO taskDto)
        {
            if (taskDto == null)
            {
                return BadRequest("Task data cannot be null.");
            }

            
            string? userId = null;
            if (!string.IsNullOrEmpty(taskDto.AssignedUserEmail))
            {
                var user = await unitOfWork.UserRepository.GetByEmailAsync(taskDto.AssignedUserEmail);
                if (user == null)
                {
                    return BadRequest("Assigned user does not exist.");
                }
                userId = user.Id;
            }

            int? teamId = null;
            if (!string.IsNullOrEmpty(taskDto.TeamName))
            {
                var team = await unitOfWork.TeamRepository.GetByNameAsync(taskDto.TeamName);
                if (team == null)
                {
                    return BadRequest("Team does not exist.");
                }
                teamId = team.Id;
            }

            var attachmentPaths = new List<string>();
            if (taskDto.Attachments != null && taskDto.Attachments.Any())
            {
                foreach (var formFile in taskDto.Attachments)
                {
                    var fileName = fileService.UploadFile(formFile);
                    if (fileName != null)
                    {
                        attachmentPaths.Add(Path.Combine("Files", fileName));
                    }
                }
            }

            var task = new TaskModel
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                PriorityLevel = taskDto.PriorityLevel,
                TaskStatus = taskDto.TaskStatus,
                AssignedUserId = userId, 
                TeamId = teamId, 
                Attachments = attachmentPaths,
                DueDate = taskDto.DueDate
            };

            try
            {
                await unitOfWork.TaskRepository.AddAsync(task);
                unitOfWork.Save();

                return Ok(new GeneralResponse
                {
                    IsSuccess = true,
                    Status = 201,
                    Data = taskDto,
                    Message = "Task created successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the task: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Leader")]
        public async Task<IActionResult> UpdateTask(int id, [FromForm] UpdateTaskDTO taskDto)
        {
            if (taskDto == null)
            {
                return BadRequest("Task data cannot be null.");
            }

            var existingTask = await unitOfWork.TaskRepository.GetByIdAsync(id);
            if (existingTask == null)
            {
                return NotFound("Task not found.");
            }

        
            existingTask.Title = taskDto.Title ?? existingTask.Title;
            existingTask.Description = taskDto.Description ?? existingTask.Description;

            if (taskDto.PriorityLevel.HasValue)
            {
                existingTask.PriorityLevel = taskDto.PriorityLevel.Value;
            }

            if (taskDto.TaskStatus.HasValue)
            {
                existingTask.TaskStatus = taskDto.TaskStatus.Value;
            }

            if (!string.IsNullOrEmpty(taskDto.AssignedUserEmail))
            {
                var user = await unitOfWork.UserRepository.GetByEmailAsync(taskDto.AssignedUserEmail);
                if (user != null)
                {
                    existingTask.AssignedUserId = user.Id;
                }
            }

            if (!string.IsNullOrEmpty(taskDto.TeamName))
            {
                var team = await unitOfWork.TeamRepository.GetByNameAsync(taskDto.TeamName);
                if (team != null)
                {
                    existingTask.TeamId = team.Id;
                }
            }

           
            if (taskDto.Attachments != null && taskDto.Attachments.Any())
            {
                var newFilePaths = new List<string>();
                foreach (var formFile in taskDto.Attachments)
                {
                    var fileName = fileService.UploadFile(formFile);
                    if (fileName != null)
                    {
                        newFilePaths.Add(Path.Combine("Files", fileName));
                    }
                }
             
                existingTask.Attachments.AddRange(newFilePaths);
            }

            try
            {
                unitOfWork.TaskRepository.Update(existingTask);
                unitOfWork.Save();

                return Ok(new GeneralResponse
                {
                    IsSuccess = true,
                    Status = 200,
                    Data = existingTask, 
                    Message = "Task updated successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the task: " + ex.Message);
            }
        }
        [Authorize(Roles = "Admin,Leader")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var task = await unitOfWork.TaskRepository.GetByIdAsync(id);
                if (task == null)
                {
                    return NotFound("Task not found.");
                }

                
                unitOfWork.TaskRepository.Delete(task);
                unitOfWork.Save();

                return Ok(new GeneralResponse
                {
                    IsSuccess = true,
                    Status = 200,
                    Message = "Task deleted successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the task: " + ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Leader")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var tasks = await unitOfWork.TaskRepository.GetAllAsync();

                return Ok(new GeneralResponse
                {
                    IsSuccess = true,
                    Status = 200,
                    Data = tasks.Select(task => new
                    {
                        id = task.Id,
                        title = task.Title,
                        description = task.Description,
                        dueDate = task.DueDate,
                        priorityLevel = task.PriorityLevel,
                        taskStatus = task.TaskStatus,
                        assignedUserId = task.AssignedUserId,
                        attachments = task.Attachments
                    }),
                    Message = "Tasks retrieved successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving tasks: " + ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                var task = await unitOfWork.TaskRepository.GetByIdAsync(id);
                if (task == null)
                {
                    return NotFound("Task not found.");
                }

                return Ok(new GeneralResponse
                {
                    IsSuccess = true,
                    Status = 200,
                    Data = new
                    {
                        id = task.Id,
                        title = task.Title,
                        description = task.Description,
                        dueDate = task.DueDate,
                        priorityLevel = task.PriorityLevel,
                        taskStatus = task.TaskStatus,
                        assignedUserId = task.AssignedUserId,
                        attachments = task.Attachments
                    },
                    Message = "Task retrieved successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the task: " + ex.Message);
            }
        }




    }
}
