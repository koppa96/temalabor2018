using Czeum.Client.Interfaces;
using Czeum.DTO.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Czeum.Client.Services {
    class UserManagerService : IUserManagerService
    {
        private string BASE_URL = "...";

        public string AccessToken => throw new NotImplementedException();
        public string Username { get; }

        public async Task LogOutAsync()
        {
            throw new NotImplementedException();
        }

        public async Task ChangePasswordAsync(ChangePasswordModel data) {
            throw new NotImplementedException();
        }

        public async Task<bool> LoginAsync(LoginModel data) {
            throw new NotImplementedException();
        }

        public async Task<bool> RegisterAsync(RegisterModel data) {
            using (var client = new HttpClient()) {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8,"application/json");
                var response = await client.PostAsync(BASE_URL, content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //Registration successfully completed
                    return true;
                }
                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    //Server error occurred
                    return false;
                }
                if(response.StatusCode == HttpStatusCode.BadRequest)
                {
                    //Malformed request
                    return false;
                }
                //Default return path, should be impossible
                return false;
            }
        }
    }
}
