using Managment_System.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.Core.DTO
{
    public class RegisterDTO
    {
        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required, StringLength(128)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [UniqueEmail(ErrorMessage = "Email is already in use")]
        public string Email { get; set; }

        [Required, StringLength(256)]
        public string Password { get; set; }

        [Required, StringLength(256)]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string RepeatPassword { get; set; }

        [Required, StringLength(15)]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
