using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ToDoList.Dtos.Entities;
using Xunit;

namespace ToDoList.Test
{
    public class ToDoNoteTest : IDisposable
    {
        private const string BASE_ADDRESS = "https://localhost:5001/";
        private const string TEST_TITLE = "[TEST_TITLE] ";

        [Fact]
        public void GetById_ToDoNode()
        {
            // Arrange
            ToDoNoteDto toDoNote1 = CreateTodoNote(new ToDoNoteDto() { Title = "GetById Test1", Items = new List<string> { "Note 1 Test 1", "Note 2 Test 1" } }); 
            ToDoNoteDto toDoNote2 = CreateTodoNote(new ToDoNoteDto() { Title = "GetById Test2", Items = new List<string> { "Note 1 Test 2", "Note 2 Test 2" } });

            // Act
            ToDoNoteDto respeonse1 = GetTodoNoteById(toDoNote1.Id);
            ToDoNoteDto respeonse2 = GetTodoNoteById(toDoNote2.Id);

            // Assert
            Assert.True(respeonse1 != null && respeonse1.Id == toDoNote1.Id);
            Assert.True(respeonse2 != null && respeonse2.Id == toDoNote2.Id);
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

        private ToDoNoteDto CreateTodoNote(ToDoNoteDto toDoNoteDto)
        {
            ToDoNoteDto itemCreated = null;

            if (string.IsNullOrEmpty(toDoNoteDto.Email))
                toDoNoteDto.Email = "unittest@gmail.com";

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
        }
    }
}
