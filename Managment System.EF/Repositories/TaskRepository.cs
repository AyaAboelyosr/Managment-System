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
    public class TaskRepository : GenerecRepository<TaskModel> , ITaskRepository
    {
        public TaskRepository(ApplicationDBContext context) : base(context) { }
        public async Task<bool> TaskExistsAsync(int taskId)
        {
            return await context.Set<TaskModel>().AnyAsync(t => t.Id == taskId);
        }

        public async Task<TaskModel?> GetByTitleAsync(string title)
        {
            return await context.TaskModels
                                 .Where(t => t.Title==title)
                                 .FirstOrDefaultAsync();
        }
    }
}
