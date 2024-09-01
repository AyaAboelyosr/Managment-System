using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Managment_System.Core.Enums;

namespace Managment_System.Core.Models
{
    public class TaskModel
    {
       
        
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime DueDate { get; set; }
            public PriorityLevel PriorityLevel { get; set; }
           public StatusOfTask TaskStatus { get; set; } 
            public string ?AssignedUserId { get; set; }
            public ApplicationUser AssignedUser { get; set; }
            public int? TeamId { get; set; }
            public Team Team { get; set; }
            public List<string> Attachments { get; set; } = new List<string>();
            public List<Comment> Comments { get; set; }
            
        }
    }

