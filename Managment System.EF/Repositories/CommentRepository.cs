using Managment_System.Core.Interfaces;
using Managment_System.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.EF.Repositories
{
   public class CommentRepository : GenerecRepository<Comment>, ICommentRepository
    
    {
       public CommentRepository(ApplicationDBContext context): base(context) { }

        public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(string userId)
        {
            return await context.Set<Comment>()
                                 .Where(c => c.UserId == userId)
                                 .ToListAsync();
        }
    }

   
}
