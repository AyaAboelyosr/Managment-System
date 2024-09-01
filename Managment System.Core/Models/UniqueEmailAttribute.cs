using Managment_System.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.Core.Models
{
  public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var unitOfWork = (IUnitOfWork)validationContext.GetService(typeof(IUnitOfWork));
            var email = value.ToString();
            var user = unitOfWork.UserRepository.GetByEmailAsync(email).Result;

            if (user != null)
            {
                return new ValidationResult("Email is already in use.");
            }

            return ValidationResult.Success;
        }

       
    }
}
