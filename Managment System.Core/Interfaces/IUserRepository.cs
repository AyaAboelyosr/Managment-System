using Managment_System.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.Core.Interfaces
{
   public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<bool> UserExistsAsync(string userId);
        Task<ApplicationUser> GetByEmailAsync(string email);
    }
}
