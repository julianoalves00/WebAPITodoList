using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Core.Entities;

namespace ToDoList.Core.Interfaces
{
    public interface IGenericRepository<T> where T : IBaseEntity
    {
        T Create(T entity);
        T GetById(int id);
        List<T> GetAll();
        void Update(T entity);
        void Delete(T entity);
        
    }
}
