using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.Core.DTO
{
  public class AddCommentDTO
    {
        public string Content { get; set; }
        public int TaskId { get; set; }
    }
}
