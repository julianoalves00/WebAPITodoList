using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
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
        #region Constructor

        public JsonDBRepository()
        {
        }

        #endregion

        #region Implementation of IGenericRepository

        public T GetById(int id)
        {
            return JsonDataBase.Instance.JsonDB.AllToDo.Where(i => i.Id == id).Cast<T>().FirstOrDefault();
        }

        public List<T> GetAll()
        {
            return new List<T>(JsonDataBase.Instance.JsonDB.AllToDo.Cast<T>());
        }

        public T Create(T entity)
        {
            T entityCreated = JsonDataBase.Instance.CreateNew(entity);

            JsonDataBase.Instance.Save();

            return entityCreated;
        }

        public void Update(T entity)
        {
            ToDoNote toDoNote = JsonDataBase.Instance.JsonDB.AllToDo.FirstOrDefault(i => i.Id == entity.Id);

            JsonDataBase.Instance.JsonDB.AllToDo.Remove(toDoNote);

            JsonDataBase.Instance.JsonDB.AllToDo.Add((ToDoNote)(IBaseEntity)entity);

            JsonDataBase.Instance.Save();
        }

        public void Delete(T entity)
        {
            IBaseEntity baseEntity = (IBaseEntity)entity;

            JsonDataBase.Instance.JsonDB.AllToDo.RemoveAll(i => i.Id == baseEntity.Id);

            JsonDataBase.Instance.Save();
        }

        #endregion
    }

}
