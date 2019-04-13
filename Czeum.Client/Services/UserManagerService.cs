﻿using Czeum.Client.Interfaces;
using Czeum.DTO.UserManagement;
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

namespace Czeum.Client.Services {
    class UserManagerService : IUserManagerService
    {
        private string BASE_URL = "...";

        public string AccessToken { get; private set; }
        private string refreshToken;
        public string Username { get; }

        public async Task<bool> LogOutAsync()
        {
            refreshToken = null;
            AccessToken = null;
            return true;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordModel data) {
            using (var client = new HttpClient()) {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try {
                    var targetUrl = Flurl.Url.Combine(BASE_URL, "/api/account/change-password");
                    var response = await client.PostAsync(targetUrl, content);
                    if (response.StatusCode == HttpStatusCode.OK) {
                        //Registration successfully completed
                        return true;
                    }

                    if (response.StatusCode == HttpStatusCode.InternalServerError) {
                        //Server error occurred
                        return false;
                    }

                    if (response.StatusCode == HttpStatusCode.BadRequest) {
                        //Malformed request
                        return false;
                    }
                }
                catch (Exception e) {
                    //Most probably timeout
                    return false;
                }
                return false;
            }
        }

        public async Task<bool> LoginAsync(LoginModel data) {
            using (var client = new HttpClient())
            {
                var formContent = new FormUrlEncodedContent(new []
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", data.Username),
                    new KeyValuePair<string, string>("password", data.Password),
                    new KeyValuePair<string, string>("scope", "api offline_access"),
                    new KeyValuePair<string, string>("client_id", "CzeumUWPClient"),
                    new KeyValuePair<string, string>("client_secret", "secret") 
                });

                try
                {
                    var targetUrl = Flurl.Url.Combine(BASE_URL, "/connect/token");
                    var response = await client.PostAsync(targetUrl, formContent);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //Login successful
                        ParseJsonResponse(await response.Content.ReadAsStringAsync());
                        return true;
                    }

                    if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        //Server error occurred
                        return false;
                    }

                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        //Incorrect credentials
                        return false;
                    }
                }
                catch (Exception e)
                {
                    //Timeout
                    return false;
                }

                return false;
            }
        }

        public async Task<bool> RegisterAsync(RegisterModel data) {
            using (var client = new HttpClient()) {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8,"application/json");

                try
                {
                    var targetUrl = Flurl.Url.Combine(BASE_URL, "/api/account/register");
                    var response = await client.PostAsync(targetUrl, content);
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

                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        //Malformed request
                        return false;
                    }
                }
                catch (Exception e)
                {
                    //Most probably timeout
                    return false;
                }
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
