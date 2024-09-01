using Managment_System.Core.Interfaces;
using Managment_System.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.EF.Repositories
{
   public class ReminderRepository : GenerecRepository<Reminder>, IReminderRepository
    
    {
        public ReminderRepository(ApplicationDBContext context) : base(context) { }
    }
}
