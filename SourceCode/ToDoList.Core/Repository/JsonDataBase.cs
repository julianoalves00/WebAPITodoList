using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ToDoList.Core.Entities;
using ToDoList.Core.Interfaces;

namespace ToDoList.Core.Repository
{
    internal class JsonDataBase
    {
        #region Constants

        private const string JSON_DB_FILE = "TodoDB.json";

        #endregion

        #region Properties

        public JsonDB JsonDB { get; set; }

        #endregion

        #region Singleton implementation

        private static JsonDataBase _instance;

        public static JsonDataBase Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new JsonDataBase();
                    _instance.LoadJsonDBFromFile();
                }
                return _instance;
            }
        }

        private JsonDataBase() { }

        #endregion

        #region Methods

        public T CreateNew<T>(T entity) where T : IBaseEntity
        {
            IBaseEntity created = null;

            entity.Id = JsonDB.GetActualId(entity.GetType().Name);
            
             Add<T>(entity);

            created = entity;

            return (T) created;
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

        public void Save()
        {
            string json = JsonConvert.SerializeObject(JsonDB, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(JSON_DB_FILE, json);
        }

        private void LoadJsonDBFromFile()
        {
            string json = File.ReadAllText(JSON_DB_FILE);
            JsonDB = JsonConvert.DeserializeObject<JsonDB>(json);

            if (JsonDB == null)
                JsonDB = new JsonDB();
        }

        #endregion
    }

    internal class JsonDB
    {
        public List<ToDoNote> ToDoNotes { get; set; }
        public List<AppUser> AppUsers { get; set; }

        private Dictionary<string, int> ActualId { get; set; }

        public JsonDB()
        {
            ToDoNotes = new List<ToDoNote>();
            AppUsers = new List<AppUser>();

            ActualId = new Dictionary<string, int>();
            ActualId.Add(typeof(ToDoNote).Name, 0);
            ActualId.Add(typeof(AppUser).Name, 0);
        }

        public int GetActualId(string typeName)
        {
            if (ActualId.ContainsKey(typeName))
                return ++ActualId[typeName];

            return 0;
        }
    }
}
