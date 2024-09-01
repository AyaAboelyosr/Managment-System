using Managment_System.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.Core.DTO
{
   public class AssignTaskDTO
    {
        public int TaskId { get; set; }
       public string? AssignedUserEmail { get; set; }
       [Required(ErrorMessage = "Task status is required.")]
        public StatusOfTask TaskStatus { get; set; }= StatusOfTask.ToDo;

       public int? TeamName { get; set; }
    }
}
