using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.Core.Interfaces
{
    public interface IEmailJob
    {
        public  Task SendScheduledEmail(string toEmail, string subject);
    }
}
