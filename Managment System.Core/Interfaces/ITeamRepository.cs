using Managment_System.Core.DTO;
using Managment_System.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.Core.Interfaces
{
    public interface ITeamRepository :IGenericRepository<Team>
    {
        Task<bool> TeamExistsAsync(int teamId);
        Task<Team> GetByNameAsync(string name);

        public  Task<IEnumerable<Team>> GetAllAsync();

        public  Task<TeamDTO> GetTeamMembersByIdAsync(int id);
        public Task RemoveMemberFromTeamAsync(int teamId, string userEmail);
    }
}
