using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ITaskRepository TaskRepository { get; }
        IUserRepository UserRepository { get; }
        ITeamRepository TeamRepository { get; }
        ICommentRepository CommentRepository { get; }

        IReminderRepository ReminderRepository { get; }

       
    


        public int Save();












       
    }
}
