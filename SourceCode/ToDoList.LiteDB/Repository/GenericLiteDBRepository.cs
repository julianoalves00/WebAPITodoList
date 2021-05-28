using LiteDB;
using System;
using System.Collections.Generic;
using ToDoList.Core.Interfaces;
using ToDoList.LiteDB.Interfaces;
using System.Linq;

namespace ToDoList.LiteDB.Repository
{
    /// <summary>
    /// Generic LiteDB Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ToDoList.Core.Interfaces.IGenericRepository{T}" />
    public class GenericLiteDBRepository<T> : IGenericRepository<T> where T : IBaseEntity
    {
        #region Properties

        private LiteDatabase _liteDb;

        #endregion

        #region Constructor

        public GenericLiteDBRepository(ILiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Database;
        }

        #endregion

        #region Implementation of IGenericRepository

        public T GetById(T entity)
        {
            IEnumerable<T> result = _liteDb.GetCollection<T>(entity.GetType().Name).Find(i => i.Id == entity.Id);
            if (result == null)
                return default;
            else
                return (new List<T>(result)).FirstOrDefault();
        }

        public List<T> Get()
        {
            IEnumerable<T> result = _liteDb.GetCollection<T>(typeof(T).Name).FindAll();
            return new List<T>(result);
        }

        public List<T> Get(Func<T, bool> filter)
        {
            IEnumerable<T> result = _liteDb.GetCollection<T>(typeof(T).Name).FindAll();

            if(result == null)
                return new List<T>();
            else
                return new List<T>(result).Where(filter).ToList();
        }

        public T Create(T entity)
        {
            BsonValue bsonValue = _liteDb.GetCollection<T>(typeof(T).Name).Insert(entity);

            if (bsonValue.AsInt32 > 0)
                return entity;
            else
                return default;
        }

        public bool Update(T entity)
        {
            return _liteDb.GetCollection<T>(typeof(T).Name).Update(entity);
        }

        public bool Delete(T entity)
        {
            return _liteDb.GetCollection<T>(typeof(T).Name).Delete(entity.Id);
        }

        #endregion
    }
}
