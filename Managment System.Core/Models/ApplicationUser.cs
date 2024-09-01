using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.Core.Models
{
public class ApplicationUser : IdentityUser
    {
        public List<TaskModel> AssignedTasks { get; set; } = new List<TaskModel>();
        public List<Team> Teams { get; set; } = new List<Team>();

    }
}
