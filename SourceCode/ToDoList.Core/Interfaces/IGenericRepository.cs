using System;
using System.Collections.Generic;

namespace ToDoList.Core.Interfaces
{
    public interface IGenericRepository<T> where T : IBaseEntity
    {
        T Create(T entity);
        T GetById(T entity);
        List<T> Get();
        List<T> Get(Func<T, bool> filter);
        void Update(T entity);
        void Delete(T entity);
        
    }
}
