using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
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

        private static JsonDataBase _instance;
        public JsonDB JsonDB { get; set; }

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

        #endregion

        public T CreateNew<T>(T entity) where T : IBaseEntity
        {
            IBaseEntity created = null;

            entity.Id = ++JsonDB.ActualId;
            entity.TimeStamp = DateTime.Now;

            if (entity is ToDoNote toDoNote)
            {
                JsonDB.AllToDo.Add(toDoNote);
                created = toDoNote;
            }

            return (T) created;
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
                JsonDB = new JsonDB(new List<ToDoNote>(), 0);
        }
    }

    internal class JsonDB
    {
        public List<ToDoNote> AllToDo { get; private set; }

        public int ActualId { get; set; }

        public JsonDB(List<ToDoNote> allToDo, int actualId)
        {
            AllToDo = allToDo;
            ActualId = actualId;
        }
    }
}
