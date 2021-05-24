using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using ToDoList.Api.Dtos.Entities;
using Xunit;

namespace ToDoList.Test
{
    public class ToDoListCRUDTest
    {
        private const string BASE_ADDRESS = "https://localhost:5001/";

        [Fact]
        public async void GetById_ToDoNode()
        {
            // Arrange
            ToDoNoteDto toDoNote1 = await CreateTodoNote(new ToDoNoteDto() { Title = "GetById Test1", Items = new List<string> { "Note 1 Test 1", "Note 2 Test 1" } }); 
            ToDoNoteDto toDoNote2 = await CreateTodoNote(new ToDoNoteDto() { Title = "GetById Test2", Items = new List<string> { "Note 1 Test 2", "Note 2 Test 2" } });

            // Act
            ToDoNoteDto respeonse1 = await GetTodoNoteById(toDoNote1.Id);
            ToDoNoteDto respeonse2 = await GetTodoNoteById(toDoNote2.Id);

            // Assert
            Assert.True(respeonse1 != null && respeonse1.Id == toDoNote1.Id);
            Assert.True(respeonse2 != null && respeonse2.Id == toDoNote2.Id);
        }

        [Fact]
        public async void GetAll_ToDoNode()
        {
            // Arrange
            List<ToDoNoteDto> listTodo = null;

            // Act
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = await client.GetAsync("todolist");

                if (response.IsSuccessStatusCode)
                {
                    listTodo = await response.Content.ReadAsAsync<List<ToDoNoteDto>>();
                }
            }

            // Assert
            Assert.True(listTodo != null && listTodo.Count > 0);
        }

        [Fact]
        public async void Create_ToDoNode()
        {
            // Arrange
            ToDoNoteDto toDoNote = new ToDoNoteDto() { Title = "Todo Note Test", Items = new List<string> { "Note 1", "Note 2" } };
            ToDoNoteDto toDoNoteCreated = null;

            // Act
            toDoNoteCreated = await CreateTodoNote(toDoNote);

            // Assert
            Assert.True(toDoNoteCreated != null && !string.IsNullOrEmpty(toDoNoteCreated.Title));
        }

        [Fact]
        public async void Update_ToDoNode()
        {
            // Arrange
            ToDoNoteDto toDoNote = await CreateTodoNote(new ToDoNoteDto() { Title = "Update Test", Items = new List<string> { "Note 1", "Note 2" } });
            toDoNote = await GetTodoNoteById(toDoNote.Id);

            // Act
            toDoNote.Title = toDoNote.Title + "Updated 1";
            toDoNote.Items.Add("Note 3");

            ToDoNoteDto toDoNoteAfterUpdate = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = await client.PutAsJsonAsync("todolist", toDoNote);

                if (response.IsSuccessStatusCode)
                    toDoNoteAfterUpdate = await GetTodoNoteById(toDoNote.Id);
            }

            // Assert
            Assert.True(toDoNoteAfterUpdate != null && toDoNote.Id == toDoNoteAfterUpdate.Id && 
                toDoNote.Title == toDoNoteAfterUpdate.Title && toDoNote.Items.Count == toDoNoteAfterUpdate.Items.Count);
        }

        [Fact]
        public async void Delete_ToDoNode()
        {
            // Arrange
            ToDoNoteDto toDoNote = await CreateTodoNote(new ToDoNoteDto() { Title = "Delete Test", Items = new List<string> { "Note 1", "Note 2" } });
            toDoNote = await GetTodoNoteById(toDoNote.Id);

            // Act
            ToDoNoteDto toDoNoteAfterDelete = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = await client.DeleteAsync($"todolist?Id={toDoNote.Id}");

                if (response.IsSuccessStatusCode)
                    toDoNoteAfterDelete = await GetTodoNoteById(toDoNote.Id);
            }

            // Assert
            Assert.True(toDoNoteAfterDelete == null);
        }

        private async Task<ToDoNoteDto> CreateTodoNote(ToDoNoteDto toDoNoteDto)
        {
            ToDoNoteDto itemCreated = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = await client.PostAsJsonAsync("todolist", toDoNoteDto);

                if (response.IsSuccessStatusCode)
                {
                    itemCreated = await response.Content.ReadAsAsync<ToDoNoteDto>();
                }
            }

            return itemCreated;
        }

        private async Task<ToDoNoteDto> GetTodoNoteById(int id)
        {
            ToDoNoteDto item = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = await client.GetAsync($"todolist/{id}");

                if (response.IsSuccessStatusCode)
                {
                    item = await response.Content.ReadAsAsync<ToDoNoteDto>();
                }
            }

            return item;
        }
    }
}
