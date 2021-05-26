using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ToDoList.Dtos.Entities;
using Xunit;

namespace ToDoList.Test
{
    public class ToDoNoteTest : IDisposable
    {
        #region Constants

        private const string BASE_ADDRESS = "https://localhost:5001/";
        private const string TEST_TITLE = "[TEST_TITLE] ";
        private const string EMAIL_APP_USER_TEST = "appusertodounittest";

        #endregion

        #region Constructor

        public ToDoNoteTest()
        {
            CreateAppUserTest(EMAIL_APP_USER_TEST);
            CreateAppUserTest(EMAIL_APP_USER_TEST + "2");
        }

        #endregion

        #region Tests Methods

        [Fact]
        public void GetById_ToDoNode()
        {
            // Arrange
            ToDoNoteDto toDoNote1 = CreateTodoNote(new ToDoNoteDto() { Title = "GetById Test1", Items = new List<string> { "Note 1 Test 1", "Note 2 Test 1" } }); 

            // Act
            ToDoNoteDto respeonse1 = GetTodoNoteById(toDoNote1.Id);

            // Assert
            Assert.True(respeonse1 != null && respeonse1.Id == toDoNote1.Id);
        }

        [Fact]
        public void GetAll_ToDoNode()
        {
            // Arrange
            List<ToDoNoteDto> listTodo = null;

            ToDoNoteDto toDoNote1 = CreateTodoNote(new ToDoNoteDto() { Title = "GetAll_ToDoNode Test1", Items = new List<string> { "Note 1 Test 1", "Note 2 Test 1" } });

            // Act
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = Task.Run(async () => await client.GetAsync("todolist")).Result;

                if (response.IsSuccessStatusCode)
                {
                    listTodo = Task.Run(async () => await response.Content.ReadAsAsync<List<ToDoNoteDto>>()).Result;
                }
            }

            // Assert
            Assert.True(listTodo != null && listTodo.Count > 0);
        }

        [Fact]
        public void GetByEmail_ToDoNode()
        {
            // Arrange
            string email2 = $"{EMAIL_APP_USER_TEST}2@gmail.com";
            ToDoNoteDto toDoNote1 = CreateTodoNote(new ToDoNoteDto() { Email = email2, Title = "GetByEmail email 2", Items = new List<string> { "Note 1 Test 1", "Note 2 Test 1" } });
            ToDoNoteDto toDoNote = CreateTodoNote(new ToDoNoteDto() { Email = email2, Title = "GetByEmail 2 email 1", Items = new List<string> { "Note 1 Test 1", "Note 2 Test 1" } });
            ToDoNoteDto toDoNote3 = CreateTodoNote(new ToDoNoteDto() { Title = "GetByEmail 3 email 2", Items = new List<string> { "Note 1 Test 1", "Note 2 Test 1" } });
            
            List<ToDoNoteDto> listTodo = null;

            // Act
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = Task.Run(async () => await client.GetAsync($"todolist?email={email2}")).Result;

                if (response.IsSuccessStatusCode)
                {
                    listTodo = Task.Run(async () => await response.Content.ReadAsAsync<List<ToDoNoteDto>>()).Result;
                }
            }

            // Assert
            Assert.True(listTodo != null && listTodo.Count == 2);
        }

        [Fact]
        public void Create_ToDoNode()
        {
            // Arrange
            ToDoNoteDto toDoNote = new ToDoNoteDto() { Title = "Todo Note Test", Items = new List<string> { "Note 1", "Note 2" } };

            // Act
            ToDoNoteDto toDoNoteCreated = CreateTodoNote(toDoNote);

            // Assert
            Assert.True(toDoNoteCreated != null && !string.IsNullOrEmpty(toDoNoteCreated.Title));
        }

        [Fact]
        public void Create_ToDoNode_Madatory_Fields()
        {
            // Arrange
            ToDoNoteDto toDoNoteWithoutTitle = new ToDoNoteDto() { Email = $"{EMAIL_APP_USER_TEST}@gmail.com", Items = new List<string> { "Note 1", "Note 2" } };
            ToDoNoteDto toDoNoteWithoutEmail = new ToDoNoteDto() { Title = "Todo Note Test", Items = new List<string> { "Note 1", "Note 2" } };

            HttpResponseMessage responseWithoutTitle = null; 
            HttpResponseMessage responseWithoutEmail = null;

            // Act
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                responseWithoutTitle = Task.Run(async () => await client.PostAsJsonAsync("todolist", toDoNoteWithoutTitle)).Result;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                responseWithoutEmail = Task.Run(async () => await client.PostAsJsonAsync("todolist", toDoNoteWithoutEmail)).Result;
            }

            // Assert
            Assert.False(responseWithoutTitle.IsSuccessStatusCode);
            Assert.False(responseWithoutEmail.IsSuccessStatusCode);
        }

        [Fact]
        public void Create_ToDoNode_AppUser_Not_Exists()
        {
            // Arrange
            ToDoNoteDto toDoNote = new ToDoNoteDto() { Email = "appuser_not_exists@mailnotexists.com", Items = new List<string> { "Note 1", "Note 2" } };

            HttpResponseMessage response = null;

            // Act
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                response = Task.Run(async () => await client.PostAsJsonAsync("todolist", toDoNote)).Result;
            }

            // Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public void Update_ToDoNode()
        {
            // Arrange
            ToDoNoteDto toDoNote = CreateTodoNote(new ToDoNoteDto() { Title = "Update Test", Items = new List<string> { "Note 1", "Note 2" } });
            toDoNote = GetTodoNoteById(toDoNote.Id);

            // Act
            toDoNote.Title = toDoNote.Title + "Updated 1";
            toDoNote.Items.Add("Note 3");

            ToDoNoteDto toDoNoteAfterUpdate = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = Task.Run(async () => await client.PutAsJsonAsync("todolist", toDoNote)).Result;

                if (response.IsSuccessStatusCode)
                    toDoNoteAfterUpdate = GetTodoNoteById(toDoNote.Id);
            }

            // Assert
            Assert.True(toDoNoteAfterUpdate != null && toDoNote.Id == toDoNoteAfterUpdate.Id && 
                toDoNote.Title == toDoNoteAfterUpdate.Title && toDoNote.Items.Count == toDoNoteAfterUpdate.Items.Count);
        }

        [Fact]
        public void Update_ToDoNode_Madatory_Fields()
        {
            // Arrange
            ToDoNoteDto toDoNoteWithoutTitle = CreateTodoNote(new ToDoNoteDto() { Title = "Update WithoutTitle", Items = new List<string> { "Note 1", "Note 2" } });
            toDoNoteWithoutTitle = GetTodoNoteById(toDoNoteWithoutTitle.Id);
            toDoNoteWithoutTitle.Title = null;

            ToDoNoteDto toDoNoteWithoutEmail = CreateTodoNote(new ToDoNoteDto() { Title = "Update WithoutTitle", Items = new List<string> { "Note 1", "Note 2" } });
            toDoNoteWithoutEmail = GetTodoNoteById(toDoNoteWithoutEmail.Id);
            toDoNoteWithoutEmail.Email = null;

            HttpResponseMessage responseWithoutTitle = null;
            HttpResponseMessage responseWithoutEmail = null;

            // Act
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                responseWithoutTitle = Task.Run(async () => await client.PutAsJsonAsync("todolist", toDoNoteWithoutTitle)).Result;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                responseWithoutEmail = Task.Run(async () => await client.PutAsJsonAsync("todolist", toDoNoteWithoutEmail)).Result;
            }

            // Assert
            Assert.False(responseWithoutTitle.IsSuccessStatusCode);
            Assert.False(responseWithoutEmail.IsSuccessStatusCode);
        }

        [Fact]
        public void Delete_ToDoNode()
        {
            // Arrange
            ToDoNoteDto toDoNote = CreateTodoNote(new ToDoNoteDto() { Title = "Delete Test", Items = new List<string> { "Note 1", "Note 2" } });

            toDoNote = GetTodoNoteById(toDoNote.Id);

            // Act
            ToDoNoteDto toDoNoteAfterDelete = null;
            
            if(RemoveTodoNote(toDoNote.Id))
                toDoNoteAfterDelete = GetTodoNoteById(toDoNote.Id);
            
            // Assert
            Assert.True(toDoNoteAfterDelete == null);
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = Task.Run(async () => await client.GetAsync("todolist")).Result;

                if (response.IsSuccessStatusCode)
                {
                    List<ToDoNoteDto> listTodo = Task.Run(async () => await response.Content.ReadAsAsync<List<ToDoNoteDto>>()).Result;

                    if (listTodo != null)
                    {
                        var listToRemove = listTodo.Where(i => i != null && i.Title.Contains(TEST_TITLE));

                        foreach (ToDoNoteDto itemRemove in listToRemove)
                            RemoveTodoNote(itemRemove.Id);
                    }
                }
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                string email = $"{EMAIL_APP_USER_TEST}@gmail.com";

                HttpResponseMessage response = Task.Run(async () => await client.DeleteAsync($"appuser?email={email}")).Result;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                string email = $"{EMAIL_APP_USER_TEST}2@gmail.com";

                HttpResponseMessage response = Task.Run(async () => await client.DeleteAsync($"appuser?email={email}")).Result;
            }
        }

        #endregion

        #region Private Methods

        private ToDoNoteDto CreateTodoNote(ToDoNoteDto toDoNoteDto)
        {
            ToDoNoteDto itemCreated = null;

            if (string.IsNullOrEmpty(toDoNoteDto.Email))
                toDoNoteDto.Email = $"{EMAIL_APP_USER_TEST}@gmail.com";

            if (!toDoNoteDto.Title.Contains(TEST_TITLE))
                toDoNoteDto.Title = TEST_TITLE + toDoNoteDto.Title;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = Task.Run(async () => await client.PostAsJsonAsync("todolist", toDoNoteDto)).Result;

                if (response.IsSuccessStatusCode)
                {
                    itemCreated = Task.Run(async () => await response.Content.ReadAsAsync<ToDoNoteDto>()).Result;
                }
            }

            Assert.False(itemCreated == null);

            return itemCreated;
        }

        private ToDoNoteDto GetTodoNoteById(int id)
        {
            ToDoNoteDto item = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = Task.Run(async () => await client.GetAsync($"todolist/{id}")).Result;

                if (response.IsSuccessStatusCode)
                {
                    item = Task.Run(async () => await response.Content.ReadAsAsync<ToDoNoteDto>()).Result;
                }
            }

            return item;
        }

        private static bool RemoveTodoNote(int id)
        {
            bool sucess = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = Task.Run(async () => await client.DeleteAsync($"todolist?Id={id}")).Result;

                sucess = response.IsSuccessStatusCode;
            }

            return sucess;
        }

        private void CreateAppUserTest(string emailName)
        {
            // Verificar se appuser testunit exists
            AppUserDto appUserTest = new AppUserDto() { Email = $"{emailName}@gmail.com", DisplayName = $"{emailName} testunit" };
            AppUserDto appUserSearch = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = Task.Run(async () => await client.GetAsync($"appuser/{appUserTest.Email}")).Result;

                if (response.IsSuccessStatusCode)
                {
                    appUserSearch = Task.Run(async () => await response.Content.ReadAsAsync<AppUserDto>()).Result;
                }
            }

            if (appUserSearch == null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(BASE_ADDRESS);

                    HttpResponseMessage response = Task.Run(async () => await client.PostAsJsonAsync("appuser", appUserTest)).Result;
                }
            }
        }

        #endregion
    }
}
