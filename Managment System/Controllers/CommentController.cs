using Managment_System.Core.DTO;
using Managment_System.Core.Interfaces;
using Managment_System.Core.Models;
using Managment_System.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public CommentController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Leader,User")]
        public async Task<IActionResult> AddComment([FromBody] AddCommentDTO commentDto)
        {
            if (commentDto == null)
            {
                return BadRequest("Comment data cannot be null.");
            }

            string userId = User.FindFirstValue("uid");

           
            Console.WriteLine($"User ID: {userId}");

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var userExists = await unitOfWork.UserRepository.UserExistsAsync(userId);
            if (!userExists)
            {
                return BadRequest("Invalid user ID for comment creation.");
            }

            var taskExists = await unitOfWork.TaskRepository.TaskExistsAsync(commentDto.TaskId);
            if (!taskExists)
            {
                return BadRequest("Task does not exist.");
            }

            var comment = new Comment
            {
                Content = commentDto.Content,
                TaskId = commentDto.TaskId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await unitOfWork.CommentRepository.AddAsync(comment);
                unitOfWork.Save();

                return Ok(new GeneralResponse
                {
                    IsSuccess = true,
                    Status = 201,
                    Data = comment,
                    Message = "Comment added successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the comment: " + ex.Message);
            }

        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Leader,User")]

        public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentDTO updateCommentDto)
        {
            if (updateCommentDto == null)
            {
                return BadRequest("Comment data cannot be null.");
            }

            string userId = User.FindFirstValue("uid");

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var comment = await unitOfWork.CommentRepository.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound("Comment not found.");
            }

        
            if (comment.UserId != userId)
            {
                return Unauthorized("You do not have permission to update this comment.");
            }

          
            comment.Content = updateCommentDto.Content;
            

            try
            {
                unitOfWork.CommentRepository.Update(comment);
                 unitOfWork.Save(); 

                return Ok(new GeneralResponse
                {
                    IsSuccess = true,
                    Status = 200,
                    Data = comment,
                    Message = "Comment updated successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the comment: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Leader,User")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            string userId = User.FindFirstValue("uid");
            var isAdmin = User.IsInRole("Admin"); 

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

           
            var comment = await unitOfWork.CommentRepository.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound("Comment not found.");
            }

           
            if (comment.UserId != userId && !isAdmin)
            {
                return Unauthorized("You do not have permission to delete this comment.");
            }

            try
            {
                unitOfWork.CommentRepository.Delete(comment);
               unitOfWork.Save(); 

                return Ok(new GeneralResponse
                {
                    IsSuccess = true,
                    Status = 200,
                    Message = "Comment deleted successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the comment: " + ex.Message);
            }
        }


        [HttpGet("user-comments")]
        [Authorize(Roles = "Admin,Leader,User")]
        public async Task<IActionResult> GetUserComments()
        {
            string userId = User.FindFirstValue("uid");
            var isAdmin = User.IsInRole("Admin"); 

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            try
            {
              
                var comments = await unitOfWork.CommentRepository.GetCommentsByUserIdAsync( userId);

                
                var commentDtos = new List<UserCommentDTO>();
                foreach (var comment in comments)
                {
                    
                    var task = await unitOfWork.TaskRepository.GetByIdAsync(comment.TaskId);

                    if (task == null)
                    {
                        continue; 
                    }

                    var commentDto = new UserCommentDTO
                    {
                      
                        Content = comment.Content,
                        CreatedAt = comment.CreatedAt,
                        titleTask= task.Title 
                    };

                    commentDtos.Add(commentDto);
                }

                return Ok(new GeneralResponse
                {
                    IsSuccess = true,
                    Status = 200,
                    Data = commentDtos,
                    Message = "Comments retrieved successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving comments: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Leader,User")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            string userId = User.FindFirstValue("uid");

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            try
            {
                var comment = await unitOfWork.CommentRepository.GetByIdAsync(id);

                if (comment == null)
                {
                    return NotFound("Comment not found.");
                }

              
                var isAdmin = User.IsInRole("Admin");
                if (comment.UserId != userId && !isAdmin)
                {
                    return Unauthorized("You do not have permission to view this comment.");
                }

                var task = await unitOfWork.TaskRepository.GetByIdAsync(comment.TaskId);

                if (task == null)
                {
                    return NotFound("Task associated with the comment not found.");
                }

           
                var commentDto = new UserCommentDTO
                {
                    Content = comment.Content,
                    CreatedAt = comment.CreatedAt,
                    titleTask = task.Title
                };

                return Ok(new GeneralResponse
                {
                    IsSuccess = true,
                    Status = 200,
                    Data = commentDto,
                    Message = "Comment retrieved successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the comment: " + ex.Message);
            }
        }






    }
}
