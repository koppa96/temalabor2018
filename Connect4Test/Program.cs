using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Connect4Dtos;
using Console = System.Console;

namespace Connect4Test {
    class Program {
        private static string token;
	    private const string Server = "https://koppa96.sch.bme.hu/Connect4Server";
	    private static HubConnection connection;
	    private static LobbyData myLobby;
	    private static List<LobbyData> lobbies = new List<LobbyData>();
	    private static MatchDto currentMatch;
	    private static List<MatchDto> myMatches;

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

		    connection.On<string>("PlayerJoinedToLobby", PlayerJoinedHandler);
		    connection.On<LobbyData>("LobbyCreated", LobbyCreatedHandler);
		    connection.On("CannotCreateLobbyFromOtherLobby", () => Console.WriteLine("Cannot create new lobby while in lobby."));
		    connection.On<LobbyData>("JoinedToLobby", JoinedToLobbyHandler);
		    connection.On("FailedToJoinLobby", FailedToJoinLobbyHandler);
		    connection.On("GuestDisconnected", GuestDisconnectedHandler);
		    connection.On<LobbyData>("LobbyAddedHandler", LobbyListChangedHandler);
		    connection.On<int>("LobbyDeleted", LobbyDeletedHandler);
		    connection.On<MatchDto>("MatchCreated", MatchCreatedHandler);
		    connection.On("NotEnoughPlayersHandler", NotEnoughPlayersHandler);
		    connection.On("IncorrectMatchIdHandler", IncorrectMatchIdHandler);
		    connection.On("ColumnFullHandler", ColumnFull);
		    connection.On("MatchFinishedHandler", MatchFinished);
		    connection.On("NotYourTurnHandler", NotYourTurnHandler);
		    connection.On<int, int>("SuccessfulPlacement", SuccessfulPlacement);
		    connection.On<int, int>("SuccessfulEnemyPlacement", SuccessfulEnemyPlacement);
		    connection.On<int, int>("VictoryHandler", VictoryHandler);
		    connection.On<int, int>("EnemyVictoryHandler", EnemyVictoryHandler);
		    await connection.StartAsync();

		    while (true) {
			    Console.Write("Connect4Test> ");
			    string command = Console.ReadLine();
			    string[] commandElements = command?.Split(' ');

			    switch (commandElements[0]) {
					case "create":
						switch (commandElements[1]) {
							case "lobby":
								commandElements[2] = commandElements[2].First().ToString().ToUpper() +
								                     commandElements[2].Substring(1);
								await connection.InvokeAsync("CreateLobby", commandElements[2]);
								break;
							case "match":
								await connection.InvokeAsync("CreateMatchAsync", myLobby.LobbyId);
								break;
							default:
								Console.WriteLine("Unknown command.");
								break;
						}
						break;
					case "join":
						switch (commandElements[1]) {
							case "lobby":
								await connection.InvokeAsync("JoinLobby", int.Parse(commandElements[2]));
								break;
							case "soloqueue":
								await connection.InvokeAsync("JoinSoloQueueAsync");
								Console.WriteLine("Joined solo queue.");
								break;
							default:
								Console.WriteLine("Unknown command");
								break;
						}
						break;
					case "disconnect":
						await connection.InvokeAsync("DisconnectFromLobby", myLobby.LobbyId);
						Console.WriteLine("Disconnected from lobby");
						break;
					case "place":
						await connection.InvokeAsync("PlaceItem", int.Parse(commandElements[1]),
							int.Parse(commandElements[2]));
						break;
					case "leave":
						await connection.InvokeAsync("LeaveSoloQueue");
						Console.WriteLine("Left solo queue");
						break;
					case "get":
						switch (commandElements[1]) {
							case "lobbies":
								lobbies = await connection.InvokeAsync<List<LobbyData>>("GetLobbies");
								Console.WriteLine("Successfully queried lobbies from server. Current lobbies: ");
								foreach (LobbyData lobby in lobbies) {
									Console.WriteLine("Id: {0}\t Host: {1}\t Status: {2}", lobby.LobbyId, lobby.Host, lobby.Status);
								}
								break;
							case "matches":
								myMatches = await connection.InvokeAsync<List<MatchDto>>("GetMatches");
								Console.WriteLine("Successfully queried matches from server. Your matches: ");
								foreach (MatchDto match in myMatches) {
									Console.WriteLine("Id: {0}\tOtherPlayer: {1}\tState: {2}", match.MatchId, match.OtherPlayer, match.State);
								}
								break;
							default:
								Console.WriteLine("Unknown command.");
								break;
						}
						break;
					default:
						Console.WriteLine("Unknown command.");
						break;
			    }
		    }
	    }

	    private static void EnemyVictoryHandler(int matchId, int columnId) {
		    Console.WriteLine("Your enemy has won Match #{0} by placing an item at Column #{1}", matchId, columnId);
	    }

	    private static void VictoryHandler(int matchId, int columnId) {
		    Console.WriteLine("You won Match #{0} by placing an item at Column #{1}", matchId, columnId);
	    }

	    private static void SuccessfulEnemyPlacement(int matchId, int columnId) {
		    Console.WriteLine("Your enemy successfully placed an item in Match #{0} at Column #{1}", matchId, columnId);
	    }

	    private static void SuccessfulPlacement(int matchId, int columnId) {
		    Console.WriteLine("Successfully placed an item in Match #{0} at Column #{1}", matchId, columnId);
	    }

	    private static void NotYourTurnHandler() {
		    Console.WriteLine("Not your turn.");
	    }

	    private static void MatchFinished() {
		    Console.WriteLine("This match has already ended.");
	    }

	    private static void ColumnFull() {
		    Console.WriteLine("This column is full.");
	    }

	    private static void IncorrectMatchIdHandler() {
		    Console.WriteLine("No match exists with that id.");
	    }

	    private static void NotEnoughPlayersHandler() {
		    Console.WriteLine("Not enough players to start a match.");
	    }

	    private static void MatchCreatedHandler(MatchDto match) {
		    currentMatch = match;
		    Console.WriteLine("Match created.");
		    Console.WriteLine("Id = {0}", match.MatchId);
			Console.WriteLine("Other player = {0}", match.OtherPlayer);
		    Console.WriteLine("Status = {0}", match.State);
	    }

	    private static void LobbyDeletedHandler(int lobbyId) {
		    lobbies.Remove(lobbies.SingleOrDefault(l => l.LobbyId == lobbyId));
		    Console.WriteLine("Lobby with id {0} was deleted.", lobbyId);
	    }

	    private static void LobbyListChangedHandler(LobbyData lobby) {
		    lobbies.Add(lobby);
		    Console.WriteLine("A new lobby was created by {0} with id {1}", lobby.Host, lobby.LobbyId);
		}

	    private static void GuestDisconnectedHandler() {
		    Console.WriteLine("{0} has disconnected from the lobby.", myLobby.Guest);
		    myLobby.Guest = null;
		}

	    private static void FailedToJoinLobbyHandler() {
		    Console.WriteLine("Could not join lobby. It is private.");
		    Console.Write("Connect4Test> ");
		}

	    private static void JoinedToLobbyHandler(LobbyData lobby) {
		    Program.myLobby = lobby;
		    Console.WriteLine("Successfully joined lobby.");
		    Console.WriteLine("Id = {0}", lobby.LobbyId);
		    Console.WriteLine("Host = {0}", lobby.Host);
			Console.WriteLine("Status = {0}", lobby.Status.ToString());
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

	    private static void PlayerJoinedHandler(string otherPlayer) {
		    myLobby.Guest = otherPlayer;
		    Console.WriteLine("{0} has joined your lobby.", otherPlayer);
		    Console.Write("Connect4Test> ");
		}

	    private static void LobbyCreatedHandler(LobbyData lobby) {
		    Program.myLobby = lobby;
		    Console.WriteLine("Lobby created.");
		    Console.WriteLine("Id = {0}", lobby.LobbyId);
		    Console.WriteLine("Status = {0}", lobby.Status.ToString());
		}
    }
}
