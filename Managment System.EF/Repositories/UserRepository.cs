using Managment_System.Core.Interfaces;
using Managment_System.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.EF.Repositories
{
    public class UserRepository : GenerecRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDBContext context) : base(context) { }

        public async Task<bool> UserExistsAsync(string userId)
        {
            return await context.Set<ApplicationUser>().AnyAsync(u => u.Id == userId);
        }

        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            return await context.Set<ApplicationUser>().FirstOrDefaultAsync(u => u.Email == email);
        }

    }
}
