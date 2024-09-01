using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Managment_System.Core.DTO;
using Managment_System.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Managment_System.Response;

namespace Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(registerDTO);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginDTO);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleDTO addRoleDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(addRoleDTO);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(addRoleDTO);
        }
        [HttpDelete("deleteUser/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var result = await _authService.DeleteUserAsync(email);

            if (string.IsNullOrEmpty(result))
            {
                return Ok(new GeneralResponse
                {
                    IsSuccess = true,
                    Status = 200,
                    Message = "User deleted successfully!"
                });
            }

            return BadRequest(new GeneralResponse
            {
                IsSuccess = false,
                Status = 400,
                Message = result
            });
        }

    }
}
