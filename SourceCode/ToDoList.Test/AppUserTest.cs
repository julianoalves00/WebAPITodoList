using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Dtos.Entities;
using Xunit;

namespace ToDoList.Test
{
    public class AppUserTest : IDisposable
    {
        private const string BASE_ADDRESS = "https://localhost:5001/";
        private const string TEST_DISPLAY_NAME = "[TEST_APP_USER] ";

        [Fact]
        public void Create_ToDoNode()
        {
            // Arrange
            AppUserDto toDoNote = new AppUserDto() { DisplayName = "User test" };

            // Act
            AppUserDto itemCreated = CreateTodoNote(toDoNote);

            // Assert
            Assert.True(itemCreated != null && !string.IsNullOrEmpty(itemCreated.DisplayName));
        }

        private AppUserDto CreateTodoNote(AppUserDto appUserDto)
        {
            AppUserDto itemCreated = null;

            if (string.IsNullOrEmpty(appUserDto.Email))
                appUserDto.Email = "unittest@gmail.com";

            if (!appUserDto.DisplayName.Contains(TEST_DISPLAY_NAME))
                appUserDto.DisplayName = TEST_DISPLAY_NAME + appUserDto.DisplayName;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = Task.Run(async () => await client.PostAsJsonAsync("appuser", appUserDto)).Result;

                if (response.IsSuccessStatusCode)
                {
                    itemCreated = Task.Run(async () => await response.Content.ReadAsAsync<AppUserDto>()).Result;
                }
            }

            Assert.False(itemCreated == null);

            return itemCreated;
        }

        private AppUserDto GetTodoNoteById(int id)
        {
            AppUserDto item = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = Task.Run(async () => await client.GetAsync($"appuser/{id}")).Result;

                if (response.IsSuccessStatusCode)
                {
                    item = Task.Run(async () => await response.Content.ReadAsAsync<AppUserDto>()).Result;
                }
            }

            return item;
        }

        public void Dispose() { }
    }
}
