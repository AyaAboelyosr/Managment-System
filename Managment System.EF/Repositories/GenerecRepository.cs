using Managment_System.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.EF.Repositories
{
    public class GenerecRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDBContext context; 

        public GenerecRepository(ApplicationDBContext Context)
        {
            this.context = Context;
        }
       

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

      
        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }


       
        public async Task<T> AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
            return entity;
        }

        public T Update(T entity)
        {
            context.Update(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }

    }
}
