using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.Core.DTO
{
   public class UserCommentDTO
    {
      
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string titleTask { get; set; }
    }
}
