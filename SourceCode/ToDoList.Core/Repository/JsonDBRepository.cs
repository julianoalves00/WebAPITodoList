using System;
using System.Collections.Generic;
using System.Linq;
using ToDoList.Core.Entities;
using ToDoList.Core.Interfaces;

namespace ToDoList.Core.Repository
{
    /// <summary>
    /// Generic JsonDB Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ToDoList.Core.Interfaces.IGenericRepository{T}" />
    public class JsonDBRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        #region Implementation of IGenericRepository

        public T GetById(T entity) 
        {
            return JsonDataBase.Instance.GetList<T>(entity).Cast<T>().Where(i => i.Id == entity.Id).Cast<T>().FirstOrDefault();
        }

        public List<T> Get()
        {
            return new List<T>(JsonDataBase.Instance.GetList<T>(typeof(T)).Cast<T>());
        }

        public List<T> Get(Func<T, bool> filter)
        {
            List<T> allRepo =  new List<T>(JsonDataBase.Instance.GetList<T>(typeof(T)).Cast<T>());

            allRepo = allRepo.Where(n => n != null).ToList();

            return allRepo.Where(filter).ToList();
        }

        public T Create(T entity)
        {
            T entityCreated = null;

            using (JsonDataBase jsonDataBase = JsonDataBase.InstanceSafe) 
            {
                entityCreated = jsonDataBase.CreateNew(entity);
            }

            return entityCreated;
        }

        public void Update(T entity)
        {
            using (JsonDataBase jsonDataBase = JsonDataBase.InstanceSafe)
            {
                IBaseEntity baseEntity = jsonDataBase.GetList<T>(entity).Cast<T>().FirstOrDefault(i => i.Id == entity.Id);

                if (baseEntity == null)
                    throw new Exception("Erro in update, entity not exist.");

                jsonDataBase.Remove(baseEntity);

                jsonDataBase.Add<T>(entity);
            }
        }

        public void Delete(T entity)
        {
            using (JsonDataBase jsonDataBase = JsonDataBase.InstanceSafe)
            {
                jsonDataBase.Remove(entity);
            }
        }

        #endregion
    }

}
