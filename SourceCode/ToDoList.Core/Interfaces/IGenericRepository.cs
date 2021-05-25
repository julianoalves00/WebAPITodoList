using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ToDoList.Core.Entities;

namespace ToDoList.Core.Interfaces
{
    public interface IGenericRepository<T> where T : IBaseEntity
    {
        T Create(T entity);
        T GetById(T entity);
        List<T> GetAll();
        List<T> GetByFilter(Func<T, bool> filter);
        void Update(T entity);
        void Delete(T entity);
        
    }
}
