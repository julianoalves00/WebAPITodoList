using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ToDoList.Core.Entities;
using ToDoList.Core.Interfaces;

namespace ToDoList.JsonDB
{
    /// <summary>
    /// Provides connection to the 'JSON database'
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    internal class JsonDataBase : IDisposable
    {
        #region Properties
        
        private string _jsonFilePath;

        public JsonDataBaseFileModel JsonDB { get; set; }

        #endregion

        #region Singleton implementation

        private static JsonDataBase _instance;

        public static JsonDataBase Instance(string jsonFilePath, bool lockInstance = false)
        {
            if (lockInstance)
                return GetInstanceAndLock(jsonFilePath);
            else
                return GetInstance(jsonFilePath);
        }

        private static JsonDataBase GetInstance(string jsonFilePath)
        {
            if (_instance == null)
            {
                _instance = new JsonDataBase(jsonFilePath);
                _instance.LoadJsonDBFromFile();
            }

            return _instance;
        }

        private JsonDataBase(string jsonFilePath) 
        {
            _jsonFilePath = jsonFilePath;
        }

        #endregion

        #region Instance thread safe
        
        private static bool lockFlag = false;

        private static JsonDataBase GetInstanceAndLock(string jsonFilePath, int count = 0)
        {
            object lockObj = new object();

            lock (lockObj)
            {
                if (lockFlag && count < 10)
                {
                    Thread.Sleep(100);
                    return GetInstanceAndLock(jsonFilePath, ++count);
                }
                
                // Lock to another thread not try write in json file in the same time
                lockFlag = true;
            }

            return GetInstance(jsonFilePath);
        }

        #endregion

        #region Public Methods

        public T CreateNew<T>(T entity) where T : IBaseEntity
        {
            entity.Id = JsonDB.GetActualId(entity.GetType().Name);
            
             Add<T>(entity);

            return (T) entity;
        }

        public bool Add<T>(T entity) where T : IBaseEntity
        {
            if (entity.Id == 0)
                return false;

            GetList<T>(entity).Add(entity);

            return true;
        }

        public bool Remove<T>(T entity) where T : IBaseEntity
        {
            T toRemove = GetList<T>(entity).Cast<T>().FirstOrDefault(i => i.Id == entity.Id);

            if (toRemove == null)
                return false;

            GetList<T>(entity).Remove(toRemove);

            return true;
        }

        public System.Collections.IList GetList<T>(T entity) where T : IBaseEntity
        {
            return GetList<T>(entity.GetType());
        }

        public System.Collections.IList GetList<T>(Type type) where T : IBaseEntity
        {
            if (type == typeof(ToDoNote))
                return JsonDB.ToDoNotes;

            if (type == typeof(AppUser))
                return JsonDB.AppUsers;

            return new List<T>();
        }

        public void Dispose()
        {
            Save();

            // Free lock to next thread can write in json file
            lockFlag = false;
        }

        #endregion

        #region Private Methods

        private void Save()
        {
            JsonDB.AppUsers = JsonDB.AppUsers.Where(n => n != null).ToList();
            JsonDB.ToDoNotes = JsonDB.ToDoNotes.Where(n => n != null).ToList();

            string json = JsonConvert.SerializeObject(JsonDB, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(_jsonFilePath, json);

            
        }

        private void LoadJsonDBFromFile()
        {
            string json = File.ReadAllText(_jsonFilePath);
            JsonDB = JsonConvert.DeserializeObject<JsonDataBaseFileModel>(json);

            if (JsonDB == null)
                JsonDB = new JsonDataBaseFileModel();
        }

        
        #endregion
    }

    #region JSON Data model

    /// <summary>
    /// Represents the data model that will be saved in the JSON file
    /// </summary>
    internal class JsonDataBaseFileModel
    {
        public List<ToDoNote> ToDoNotes { get; set; }
        public List<AppUser> AppUsers { get; set; }
        public Dictionary<string, int> ActualIds { get; set; }

        public JsonDataBaseFileModel()
        {
            ToDoNotes = new List<ToDoNote>();
            AppUsers = new List<AppUser>();

            ActualIds = new Dictionary<string, int>();
            ActualIds.Add(typeof(ToDoNote).Name, 0);
            ActualIds.Add(typeof(AppUser).Name, 0);
        }

        public int GetActualId(string typeName)
        {
            if (ActualIds.ContainsKey(typeName))
                return ++ActualIds[typeName];

            return 0;
        }
    }

    #endregion
}
