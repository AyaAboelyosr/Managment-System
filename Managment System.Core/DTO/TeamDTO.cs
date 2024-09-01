using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.Core.DTO
{
   public class TeamDTO
    {
        public string Name { get; set; }
        public List<string> MemberUserNames { get; set; }
    }
}
