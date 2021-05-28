using System;
using System.Collections.Generic;
using System.Linq;
using ToDoList.Core.Interfaces;
using ToDoList.JsonDB.Interfaces;

namespace ToDoList.JsonDB.Repository
{
    /// <summary>
    /// Generic JsonDB Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ToDoList.Core.Interfaces.IGenericRepository{T}" />
    public class GenericJsonDBRepository<T> : IGenericRepository<T> where T : IBaseEntity
    {
        #region Properties

        private string _jsonFilePath;

        #endregion

        #region Constructor

        public GenericJsonDBRepository(IJSonDBOptions jsonDBOptions)
        {
            _jsonFilePath = jsonDBOptions.JsonFilePath;
        }

        #endregion

        #region Methods

        private JsonDataBase GetJsonDataBase(bool lockInstance = true)
        {
            return JsonDataBase.Instance(_jsonFilePath, lockInstance);
        }

        #endregion

        #region Implementation of IGenericRepository

        public T GetById(T entity) 
        {
            return GetJsonDataBase(false).GetList<T>(entity).Cast<T>().Where(i => i.Id == entity.Id).Cast<T>().FirstOrDefault();
        }

        public List<T> Get()
        {
            return new List<T>(GetJsonDataBase(false).GetList<T>(typeof(T)).Cast<T>());
        }

        public List<T> Get(Func<T, bool> filter)
        {
            List<T> allRepo =  new List<T>(GetJsonDataBase(false).GetList<T>(typeof(T)).Cast<T>());

            allRepo = allRepo.Where(n => n != null).ToList();

            return allRepo.Where(filter).ToList();
        }

        public T Create(T entity)
        {
            T entityCreated = default(T);

            using (JsonDataBase jsonDataBase = GetJsonDataBase()) 
            {
                entityCreated = jsonDataBase.CreateNew(entity);
            }

            return entityCreated;
        }

        public bool Update(T entity)
        {
            bool sucess = false;

            using (JsonDataBase jsonDataBase = GetJsonDataBase())
            {
                IBaseEntity baseEntity = jsonDataBase.GetList<T>(entity).Cast<T>().FirstOrDefault(i => i.Id == entity.Id);

                if (baseEntity == null)
                    throw new Exception("Erro in update, entity not exist.");

                sucess = jsonDataBase.Remove(baseEntity);

                if(sucess)
                    sucess = jsonDataBase.Add<T>(entity);
            }

            return sucess;
        }

        public bool Delete(T entity)
        {
            bool sucess = false;

            using (JsonDataBase jsonDataBase = GetJsonDataBase())
            {
                sucess = jsonDataBase.Remove(entity);
            }

            return sucess;
        }

        #endregion
    }
}
