using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Test {
    class Program {
        static string token;
		static readonly string server = "https://koppa96.sch.bme.hu/Connect4Server";

		static void Main(string[] args) {
            MainAsync();
            Console.ReadKey();
        }

        static async void MainAsync() {
            token = await LoginAsync();
			TestHub(token);
        }

        static async void TestHub(string token) {
            HubConnection connection = new HubConnectionBuilder()
                                       .WithUrl(server + "/gamehub", options => {
										   options.AccessTokenProvider = () => Task.FromResult(token);
									   })
                                       .Build();
			connection.On<int>("LobbyCreated", (lobbyId) => { Console.WriteLine("Lobby Created with id: {0}", lobbyId); });

            await connection.StartAsync();

			await connection.InvokeAsync("CreateLobbyAsync", "Public");
        }

        static async Task<string> LoginAsync() {
            JObject jObject = new JObject {
                { "Username", "testuser" },
                { "Password", "Alma.123" }
            };

            string json = jObject.ToString();
            string url = server + "/Account/Login";
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

			HttpClientHandler handler = new HttpClientHandler();

            using (HttpClient client = new HttpClient()) {
				ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, ssl) => true;
                HttpResponseMessage responseMessage = await client.PostAsync(url, content);

                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK) {
                    return await responseMessage.Content.ReadAsStringAsync();
                }
            }

            return "Something went wrong";
        }

        static async Task<string> RegisterAsync() {
            JObject jObject = new JObject {
                { "Username", "almaUser" },
                { "Email", "alma@alma.alma" },
                { "Password", "Alma.123" },
                { "ConfirmPassword", "Alma.123" }
            };

            string json = jObject.ToString();
            string url = server + "/Account/Register";
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient()) {
                HttpResponseMessage responseMessage = await client.PostAsync(url, content);

                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK) {
                    string token = await responseMessage.Content.ReadAsStringAsync();

                    return token;
                }
            }

            return "Something went wrong";
        }
    }
}
