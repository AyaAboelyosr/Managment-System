using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.Core.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ApplicationUser> Members { get; set; }
        public List<TaskModel> Tasks { get; set; } 
    }
}
