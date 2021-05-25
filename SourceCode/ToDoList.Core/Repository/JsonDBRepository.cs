﻿using Newtonsoft.Json;
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

        public T GetById(T entity) 
        {
            return JsonDataBase.Instance.GetList<T>(entity).Cast<T>().Where(i => i.Id == entity.Id).Cast<T>().FirstOrDefault();
        }

        public List<T> GetAll()
        {
            return new List<T>(JsonDataBase.Instance.GetList<T>(typeof(T)).Cast<T>());
        }

        public T Create(T entity)
        {
            T entityCreated = JsonDataBase.Instance.CreateNew(entity);

            JsonDataBase.Instance.Save();

            return entityCreated;
        }

        public void Update(T entity)
        {
            IBaseEntity baseEntity = JsonDataBase.Instance.GetList<T>(entity).Cast<T>().FirstOrDefault(i => i.Id == entity.Id);

            JsonDataBase.Instance.Remove(baseEntity);

            JsonDataBase.Instance.Add<T>(entity);

            JsonDataBase.Instance.Save();
        }

        public void Delete(T entity)
        {
            JsonDataBase.Instance.Remove(entity);

            JsonDataBase.Instance.Save();
        }

        #endregion
    }

}