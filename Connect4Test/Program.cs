using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Test {
    class Program {
        static string token;

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
                                       .WithUrl("https://localhost:44301/gamehub", options => {
                                           options.AccessTokenProvider = () => Task.FromResult(token);
                                       })
                                       .Build();

            await connection.StartAsync();

            string response = (string) await connection.InvokeCoreAsync("Hello", typeof(string), new[] { "it's me" });

            Console.WriteLine($"{response}");
        }

        static async Task<string> LoginAsync() {
            JObject jObject = new JObject {
                { "Username", "testuser" },
                { "Password", "Alma.123" }
            };

            string json = jObject.ToString();
            string url = "https://localhost:44301/Account/Login";
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient()) {
                HttpResponseMessage responseMessage = await client.PostAsync(url, content);

                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK) {
                    string token = await responseMessage.Content.ReadAsStringAsync();

                    Console.WriteLine("Logged in successfully. Access token: {0}", token);
                    return token;
                }
            }

            return "Something went wrong";
        }

        static async void PrintTokenAsync() {
            string token = await RegisterAsync();
        }

        static async Task<string> RegisterAsync() {
            JObject jObject = new JObject {
                { "Username", "almaUser" },
                { "Email", "alma@alma.alma" },
                { "Password", "Alma.123" },
                { "ConfirmPassword", "Alma.123" }
            };

            string json = jObject.ToString();
            string url = "https://localhost:44301/Account/Register";
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
