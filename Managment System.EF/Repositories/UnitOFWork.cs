
using Managment_System.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Managment_System.EF.Repositories
{
    public class UnitOFWork :  IUnitOfWork
    {
        private readonly ApplicationDBContext context;
        public ITaskRepository TaskRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }
        public ITeamRepository TeamRepository { get; private set; }
        public ICommentRepository CommentRepository { get; private set; }

        public IReminderRepository ReminderRepository { get; private set; } 


        public UnitOFWork(ApplicationDBContext _context)
        {
            context = _context;
            TaskRepository = new TaskRepository(context);
            UserRepository = new UserRepository(context);
            TeamRepository = new TeamRepository(context);
            CommentRepository= new CommentRepository(context); 
            ReminderRepository = new ReminderRepository(context);



        }
   


       




        public int Save()
        {
            return context.SaveChanges();
        }


        public void Dispose()
        {
            context.Dispose();
        }

    }
}
