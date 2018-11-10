using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Connect4Dtos;
using Console = System.Console;

namespace Connect4Test {
    class Program {
        private static string token;
	    private const string Server = "https://localhost:44301";
	    private static HubConnection connection;
	    private static int lobbyId;

		static void Main(string[] args) {
			MainAsync().Wait();
		}

	    private static async Task MainAsync() {
		    Console.Write("Username: ");
		    string username = Console.ReadLine();
			Console.Write("Password: ");
		    string password = Console.ReadLine();

		    token = await LoginAsync(username, password);
		    connection = new HubConnectionBuilder().WithUrl(Server + "/gamehub", options => {
			    options.AccessTokenProvider = () => Task.FromResult(token);
		    }).Build();

		    connection.On<int>("LobbyCreated", LobbyCreatedHandler);
		    connection.On<string>("PlayerJoinedToLobby", PlayerJoinedHandler);
		    await connection.StartAsync();


		    List<MatchDto> matches = await connection.InvokeAsync<List<MatchDto>>("GetMatches");

		    Console.WriteLine("This player has {0} matches", matches.Count);
		    Console.ReadKey();
	    }

        private static async Task<string> LoginAsync(string username, string password) {
            JObject jObject = new JObject {
                { "Username", username },
                { "Password", password }
            };

            string json = jObject.ToString();
            string url = Server + "/Account/Login";
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

			HttpClientHandler handler = new HttpClientHandler();

            using (HttpClient client = new HttpClient()) {
				ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, ssl) => true;
                HttpResponseMessage responseMessage = await client.PostAsync(url, content);

                if (responseMessage.StatusCode == HttpStatusCode.OK) {
	                Console.WriteLine("Successful login");
                    return await responseMessage.Content.ReadAsStringAsync();
                }
            }

            return "Something went wrong";
        }

        private static async Task<string> RegisterAsync() {
            JObject jObject = new JObject {
                { "Username", "almaUser" },
                { "Email", "alma@alma.alma" },
                { "Password", "Alma.123" },
                { "ConfirmPassword", "Alma.123" }
            };

            string json = jObject.ToString();
            string url = Server + "/Account/Register";
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

	    private static void LobbyCreatedHandler(int lobbyId) {
		    Program.lobbyId = lobbyId;
			Console.WriteLine("Lobby successfully created with id: {0}", lobbyId);
	    }

	    private static void PlayerJoinedHandler(string otherPlayer) {
		    Console.WriteLine("{0} has joined your lobby.", otherPlayer);
	    }
    }
}
