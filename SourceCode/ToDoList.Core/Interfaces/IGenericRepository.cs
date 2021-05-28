using System;
using System.Collections.Generic;

namespace ToDoList.Core.Interfaces
{
    /// <summary>
    /// Generic interface to repository methods
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRepository<T> where T : IBaseEntity
    {
        T Create(T entity);
        T GetById(T entity);
        List<T> Get();
        List<T> Get(Func<T, bool> filter);
        bool Update(T entity);
        bool Delete(T entity);
    }
}
