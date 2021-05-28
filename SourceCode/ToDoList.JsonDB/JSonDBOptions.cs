using ToDoList.JsonDB.Interfaces;

namespace ToDoList.JsonDB
{
    public class JSonDBOptions : IJSonDBOptions
    {
        public string JsonFilePath { get; set; }

        public JSonDBOptions(string jsonFilePath)
        {
            JsonFilePath = jsonFilePath;
        }
    }
}
