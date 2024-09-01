using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.Core.Interfaces
{
   public interface IEmailService
    {
        Task sendEmailAsync(string mailTo, string subject, string body = null, IList<IFormFile> attatchmemts = null);
    }
}
