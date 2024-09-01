using Managment_System.Core.DTO;
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
    public class TeamRepository : GenerecRepository<Team>, ITeamRepository
    {
        public TeamRepository(ApplicationDBContext context) : base(context) { }

        public async Task<bool> TeamExistsAsync(int teamId)
        {
            return await context.Set<Team>().AnyAsync(t => t.Id == teamId);
        }
        public async Task<Team> GetByNameAsync(string name)
        {
           
            return await context.Set<Team>()
                .FirstOrDefaultAsync(t => t.Name==name);
        }

        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            return await context.Teams
                .Include(t => t.Members) 
                .ToListAsync();
        }

        public async Task<TeamDTO> GetTeamMembersByIdAsync(int id)
        {
            return await context.Teams
                .Where(t => t.Id == id)
                .Select(t => new TeamDTO
                {
                    Name = t.Name,
                    MemberUserNames = t.Members.Select(m => m.UserName).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task RemoveMemberFromTeamAsync(int teamId, string userEmail)
        {
            // Load the team including its members
            var team = await context.Teams
                .Include(t => t.Members)
                .FirstOrDefaultAsync(t => t.Id == teamId);

            if (team == null)
            {
                throw new ArgumentException("Team not found");
            }

            // Find the member to remove
            var member = team.Members.FirstOrDefault(m => m.Email == userEmail);

            if (member != null)
            {
                team.Members.Remove(member);
                context.Teams.Update(team);
                await context.SaveChangesAsync();
            }
        }
    }
}
