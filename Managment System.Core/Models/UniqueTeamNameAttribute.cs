using Managment_System.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.Core.Models
{
   public class UniqueTeamNameAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var unitOfWork = (IUnitOfWork)validationContext.GetService(typeof(IUnitOfWork));
            var teamName = value.ToString();

            if (string.IsNullOrEmpty(teamName))
            {
                return ValidationResult.Success;
            }

            
            var team = unitOfWork.TeamRepository.GetByNameAsync(teamName).Result;

            if (team != null)
            {
                return new ValidationResult("Team name is already in use.");
            }

            return ValidationResult.Success;
        }
    }
}
