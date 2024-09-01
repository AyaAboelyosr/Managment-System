using Managment_System.Core.Enums;
using Managment_System.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.Core.DTO
{
    public class UpdateTaskDTO
    {
        
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; }

       
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        public PriorityLevel? PriorityLevel { get; set; }
        public List<IFormFile> Attachments { get; set; } = new List<IFormFile>();

        public StatusOfTask? TaskStatus { get; set; }

        public string AssignedUserEmail { get; set; }


        [DataType(DataType.DateTime, ErrorMessage = "Due date must be a valid date and time.")]
        public DateTime DueDate { get; set; }
        public string? TeamName { get; set; }
    }
}
