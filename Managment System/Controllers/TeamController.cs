using Managment_System.Core.DTO;
using Managment_System.Core.Interfaces;
using Managment_System.Core.Models;
using Managment_System.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public TeamController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Leader")]
        public async Task<IActionResult> CreateTeam([FromBody] AddTeamDTO addTeamDTO)
        {
            if (addTeamDTO == null)
            {
                return BadRequest(new GeneralResponse
                {
                    IsSuccess = false,
                    Status = 400,
                    Message = "Team data cannot be null."
                });
            }

            var validMembers = new List<ApplicationUser>();
            if (addTeamDTO.MemberEmails != null && addTeamDTO.MemberEmails.Any())
            {
                foreach (var email in addTeamDTO.MemberEmails)
                {
                    var user = await unitOfWork.UserRepository.GetByEmailAsync(email);
                    if (user == null)
                    {
                        return BadRequest(new GeneralResponse
                        {
                            IsSuccess = false,
                            Status = 400,
                            Message = $"User with email {email} does not exist."
                        });
                    }
                    validMembers.Add(user);
                }
            }

            var team = new Team
            {
                Name = addTeamDTO.Name,
                Members = validMembers
            };

            try
            {
                await unitOfWork.TeamRepository.AddAsync(team);
                unitOfWork.Save();

                
                var teamDTO = new TeamDTO
                {
                    Name = team.Name,
                    MemberUserNames = team.Members.Select(m => m.UserName).ToList()
                };

                return Ok(new GeneralResponse
                {
                    IsSuccess = true,
                    Status = 201,
                    Data = teamDTO,
                    Message = "Team created successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponse
                {
                    IsSuccess = false,
                    Status = 500,
                    Message = "An error occurred while creating the team: " + ex.Message
                });
            }
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Leader")]
        public async Task<IActionResult> UpdateTeam(int id, [FromBody] UpdateTeamDTO updateTeamDTO)
        {
            if (updateTeamDTO == null)
            {
                return BadRequest(new GeneralResponse
                {
                    IsSuccess = false,
                    Status = 400,
                    Message = "Team data cannot be null."
                });
            }

            var team = await unitOfWork.TeamRepository.GetByIdAsync(id);
            if (team == null)
            {
                return NotFound(new GeneralResponse
                {
                    IsSuccess = false,
                    Status = 404,
                    Message = "Team not found."
                });
            }

            
            if (!string.IsNullOrWhiteSpace(updateTeamDTO.Name))
            {
                team.Name = updateTeamDTO.Name;
            }

        
            if (updateTeamDTO.MemberEmails != null)
            {
               
                if (team.Members == null)
                {
                    team.Members = new List<ApplicationUser>();
                }

                var existingMemberEmails = team.Members.Select(m => m.Email).ToList();

                foreach (var email in updateTeamDTO.MemberEmails)
                {
                    if (!existingMemberEmails.Contains(email))
                    {
                        var user = await unitOfWork.UserRepository.GetByEmailAsync(email);
                        if (user == null)
                        {
                            return BadRequest(new GeneralResponse
                            {
                                IsSuccess = false,
                                Status = 400,
                                Message = $"User with email {email} does not exist."
                            });
                        }

                        if (!team.Members.Contains(user))
                        {
                            team.Members.Add(user);
                        }
                    }
                }
            }

            try
            {
               unitOfWork.Save();

                var teamDTO = new TeamDTO
                {
                    Name = team.Name,
                    MemberUserNames = team.Members?.Select(m => m.UserName).ToList() ?? new List<string>()
                };

                return Ok(new GeneralResponse
                {
                    IsSuccess = true,
                    Status = 200,
                    Data = teamDTO,
                    Message = "Team updated successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponse
                {
                    IsSuccess = false,
                    Status = 500,
                    Message = "An error occurred while updating the team: " + ex.Message
                });
            }
        }






        [HttpGet]
        [Authorize(Roles = "Admin,Leader")]
        public async Task<IActionResult> GetAllTeams()
        {
            try
            {
                var teams = await unitOfWork.TeamRepository.GetAllAsync();

                if (teams == null)
                {
                   
                    return NotFound(new GeneralResponse
                    {
                        IsSuccess = false,
                        Status = 404,
                        Message = "No teams found."
                    });
                }

               var teamDTOs = teams.Select(team => new TeamDTO
                {
                    Name = team.Name,
                    MemberUserNames = team.Members?.Select(m => m.UserName).ToList() ?? new List<string>() 
                }).ToList();

                return Ok(new GeneralResponse
                {
                    IsSuccess = true,
                    Status = 200,
                    Data = teamDTOs,
                    Message = "Teams retrieved successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponse
                {
                    IsSuccess = false,
                    Status = 500,
                    Message = "An error occurred while retrieving teams: " + ex.Message
                });
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Leader")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await unitOfWork.TeamRepository.GetByIdAsync(id);
            if (team == null)
            {
                return NotFound(new GeneralResponse
                {
                    IsSuccess = false,
                    Status = 404,
                    Message = "Team not found."
                });
            }

            try
            {
                unitOfWork.TeamRepository.Delete(team);
                unitOfWork.Save();

                return Ok(new GeneralResponse
                {
                    IsSuccess = true,
                    Status = 200,
                    Message = "Team deleted successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponse
                {
                    IsSuccess = false,
                    Status = 500,
                    Message = "An error occurred while deleting the team: " + ex.Message
                });
            }
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Leader")]
        public async Task<IActionResult> GetTeamById(int id)
        {
            try
            {
                var teamDTO = await unitOfWork.TeamRepository.GetTeamMembersByIdAsync(id);
                if (teamDTO == null)
                {
                    return NotFound(new GeneralResponse
                    {
                        IsSuccess = false,
                        Status = 404,
                        Message = "Team not found."
                    });
                }

                return Ok(new GeneralResponse
                {
                    IsSuccess = true,
                    Status = 200,
                    Data = teamDTO,
                    Message = "Team retrieved successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponse
                {
                    IsSuccess = false,
                    Status = 500,
                    Message = "An error occurred while retrieving the team: " + ex.Message
                });
            }
        }

        [HttpDelete("{teamId}/members/{userEmail}")]
        [Authorize(Roles = "Admin,Leader")]
        public async Task<IActionResult> RemoveMember(int teamId, string userEmail)
        {
            try
            {
                await unitOfWork.TeamRepository.RemoveMemberFromTeamAsync(teamId, userEmail);

                return Ok(new GeneralResponse
                {
                    IsSuccess = true,
                    Status = 200,
                    Message = "Member removed successfully!"
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new GeneralResponse
                {
                    IsSuccess = false,
                    Status = 404,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponse
                {
                    IsSuccess = false,
                    Status = 500,
                    Message = "An error occurred while removing the member: " + ex.Message
                });
            }
        }










    }
}
