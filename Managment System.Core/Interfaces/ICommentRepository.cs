using Managment_System.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.Core.Interfaces
{
    public interface ICommentRepository :IGenericRepository<Comment>
    {
        public Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(string userId);
    }
}
