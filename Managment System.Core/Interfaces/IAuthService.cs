using Managment_System.Core.DTO;
using Managment_System.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthDTO> RegisterAsync(RegisterDTO registerDTO);
        Task<AuthDTO> LoginAsync(LoginDTO loginDTO);
        Task<string> AddRoleAsync(AddRoleDTO addRoleDTO);

        public  Task<string> DeleteUserAsync(string email);
    }
}
