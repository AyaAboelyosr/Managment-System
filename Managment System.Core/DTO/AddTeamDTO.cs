using Managment_System.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.Core.DTO
{
   public class AddTeamDTO
    {
        [UniqueTeamName]
        [Required(ErrorMessage = "Name is required.")]

        public string Name { get; set; }
        public List<string> MemberEmails { get; set; }
    }
}
