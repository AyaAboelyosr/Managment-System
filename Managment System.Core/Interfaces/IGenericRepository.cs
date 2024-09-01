using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Managment_System.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {

       

       
        public Task<IEnumerable<T>> GetAllAsync();

        

        
        public Task<T> GetByIdAsync(int id);

  
       
        public Task<T> AddAsync(T entity);

       
        public T Update(T entity);

   
        public void Delete(T entity);





    }
}
