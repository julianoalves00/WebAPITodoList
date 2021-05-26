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
    public class AppUserTest : IDisposable
    {
        #region Constants

        private const string BASE_ADDRESS = "https://localhost:5001/";
        private const string TEST_DISPLAY_NAME = "[TEST_APP_USER] ";
        private const string EMAIL_APP_USER_TEST = "unittestappuser";

        #endregion

        #region Tests Methods

        [Fact]
        public void Create_AppUser()
        {
            // Arrange
            AppUserDto toDoNote = new AppUserDto() { DisplayName = "User test" };

            // Act
            AppUserDto itemCreated = CreateAppUser(toDoNote);

            // Assert
            Assert.True(itemCreated != null && !string.IsNullOrEmpty(itemCreated.DisplayName));
        }

        [Fact]
        public void GetByEmail_AppUser()
        {
            // Arrange
            AppUserDto appUser1 = CreateAppUser(new AppUserDto() { DisplayName = "app user 1" });

            // Act
            AppUserDto respeonse1 = GetAppUserByEmail(appUser1.Email);

            // Assert
            Assert.True(respeonse1 != null && respeonse1.Email == appUser1.Email);
        }

        [Fact]
        public void GetAll_AppUser()
        {
            // Arrange
            List<ToDoNoteDto> listTodo = null;

            AppUserDto appUser1 = CreateAppUser(new AppUserDto() { DisplayName = "GetAll_AppUser Test1" });

            // Act
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = Task.Run(async () => await client.GetAsync("appuser")).Result;

                if (response.IsSuccessStatusCode)
                {
                    listTodo = Task.Run(async () => await response.Content.ReadAsAsync<List<ToDoNoteDto>>()).Result;
                }
            }

            // Assert
            Assert.True(listTodo != null && listTodo.Count > 0);
        }

        [Fact]
        public void Create_AppUser_Mandatory_Fields()
        {
            // Arrange
            AppUserDto appUserWithoutEmail = new AppUserDto { DisplayName = "app User Without Email" };
            AppUserDto appUserWithoutDisplayName = new AppUserDto { Email = string.Format("test{0:HHmmssfff}@gmail.com", DateTime.Now) };

            HttpResponseMessage responseWithoutEmail = null;
            HttpResponseMessage responseWithoutDisplayName = null;

            // Act
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                responseWithoutEmail = Task.Run(async () => await client.PostAsJsonAsync("appuser", appUserWithoutEmail)).Result;
                //Thread.Sleep(100);
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                responseWithoutDisplayName = Task.Run(async () => await client.PostAsJsonAsync("appuser", appUserWithoutDisplayName)).Result;
                //Thread.Sleep(100);
            }

            // Assert
            Assert.False(responseWithoutEmail.IsSuccessStatusCode);
            Assert.False(responseWithoutDisplayName.IsSuccessStatusCode);
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = Task.Run(async () => await client.GetAsync("appuser")).Result;

                if (response.IsSuccessStatusCode)
                {
                    List<AppUserDto> listAppUser = Task.Run(async () => await response.Content.ReadAsAsync<List<AppUserDto>>()).Result;

                    if (listAppUser != null)
                    {
                        var listToRemove = listAppUser.Where(i => i != null && i.Email.Contains(EMAIL_APP_USER_TEST));

                        foreach (AppUserDto itemRemove in listToRemove)
                            RemoveAppUser(itemRemove.Email);
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private AppUserDto CreateAppUser(AppUserDto appUserDto)
        {
            AppUserDto itemCreated = null;

            if (string.IsNullOrEmpty(appUserDto.Email))
                appUserDto.Email = string.Format("{0}{1:HHmmssfff}@gmail.com", EMAIL_APP_USER_TEST, DateTime.Now);

            if (!appUserDto.DisplayName.Contains(TEST_DISPLAY_NAME))
                appUserDto.DisplayName = TEST_DISPLAY_NAME + appUserDto.DisplayName;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = Task.Run(async () => await client.PostAsJsonAsync("appuser", appUserDto)).Result;
                //Thread.Sleep(100);

                if (response.IsSuccessStatusCode)
                {
                    itemCreated = Task.Run(async () => await response.Content.ReadAsAsync<AppUserDto>()).Result;
                }
            }

            Assert.False(itemCreated == null);

            return itemCreated;
        }

        private AppUserDto GetAppUserByEmail(string email)
        {
            AppUserDto item = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = Task.Run(async () => await client.GetAsync($"appuser/{email}")).Result;

                if (response.IsSuccessStatusCode)
                {
                    item = Task.Run(async () => await response.Content.ReadAsAsync<AppUserDto>()).Result;
                }
            }

            return item;
        }

        private static bool RemoveAppUser(string email)
        {
            bool sucess = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);

                HttpResponseMessage response = Task.Run(async () => await client.DeleteAsync($"appuser?email={email}")).Result;

                sucess = response.IsSuccessStatusCode;
            }

            return sucess;
        }

        #endregion
    }
}
