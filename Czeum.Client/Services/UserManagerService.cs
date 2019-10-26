using Czeum.Client.Interfaces;
using Czeum.Core.DTOs.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Flurl;
using Flurl.Http;

namespace Czeum.Client.Services {
    public class UserManagerService : IUserManagerService
    {
        //private string BASE_URL = "https://localhost:5001";
        private string BASE_URL = App.Current.Resources["BaseUrl"].ToString();

        private IDialogService dialogService;

        public string AccessToken { get; private set; }
        private string refreshToken;

        public string Username { get; }

        public async Task<bool> LogOutAsync()
        {
            refreshToken = null;
            AccessToken = null;
            return true;
        }

        public UserManagerService(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordModel data) 
        {
            try
            {
                await BASE_URL.AppendPathSegment("api/account/change-password").PostJsonAsync(data).ReceiveString();
                return true;
            }
            catch (FlurlHttpException e)
            {
                await dialogService.ShowError("Changing password failed");
                return false;
            }
        }

        public async Task<bool> LoginAsync(LoginModel data)
        {
            try
            {
                var result = await BASE_URL.AppendPathSegment("connect/token").PostUrlEncodedAsync(new
                {
                    grant_type = "password",
                    username = data.Username,
                    password = data.Password,
                    scope = "czeum_api offline_access",
                    client_id = "CzeumUWPClient",
                    client_secret = "UWPClientSecret"
                }).ReceiveString();

                ParseJsonResponse(result);
                return true;

            } catch (FlurlHttpException e)
            {
                await dialogService.ShowError("Login failed");
                return false;
            }
        }

        public async Task<bool> RegisterAsync(RegisterModel data)
        {
            try
            {
                await BASE_URL.AppendPathSegment("api/accounts/register").PostJsonAsync(data);
                return true;

            }
            catch (FlurlHttpException e)
            {
                await dialogService.ShowError("Registration failed");
                return false;
            }
        }
        public async Task<bool> ConfirmAsync(string name, string confirmationToken)
        {
            try
            {
                await BASE_URL.AppendPathSegment("api/accounts/confirm-email").SetQueryParams(new { username = name, token = confirmationToken }).PostAsync(null).ReceiveString();
                return true;
            } 
            catch (FlurlHttpException e)
            {
                await dialogService.ShowError("Email confirmation failed");
                return false;
            }
        }

        private void ParseJsonResponse(string jsonString)
        {
            var jsonObject = JObject.Parse(jsonString);
            AccessToken = jsonObject.GetValue("access_token").ToString();
            refreshToken = jsonObject.GetValue("refresh_token").ToString();
        }

    }
}
