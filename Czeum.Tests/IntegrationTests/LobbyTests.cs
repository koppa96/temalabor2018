using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Czeum.Api.Common;
using Czeum.Core.DTOs.Lobbies;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Enums;
using Czeum.DAL;
using Czeum.Domain.Entities;
using Czeum.Tests.IntegrationTests.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Czeum.Tests.IntegrationTests
{
    [TestClass]
    public class LobbyTests
    {
        private CzeumFactory factory;
        private HttpClient client;
        
        [TestInitialize]
        public async Task SetUpWebHost()
        {
            factory = new CzeumFactory();
            client = factory.CreateClient();
            await factory.SeedUsersAsync();
        }

        private async Task<LobbyDataWrapper> CreateLobbyAs(LobbyAccess lobbyAccess, string user)
        {
            var response = await client.PostJsonAsync(
                "api/lobbies",
                new CreateLobbyDto
                {
                    GameType = GameType.Chess,
                    LobbyAccess = lobbyAccess,
                    Name = "Teszt lobby"
                },
                user);

            response.IsSuccessStatusCode.Should().BeTrue();
            return JsonConvert.DeserializeObject<LobbyDataWrapper>(await response.Content.ReadAsStringAsync());
        }

        [TestMethod]
        public async Task CreateLobbyWorks()
        {
            await CreateLobbyAs(LobbyAccess.Public, "teszt1");

            var lobbies = await client.GetJsonAsync<IEnumerable<LobbyDataWrapper>>("api/lobbies", "teszt1");
            lobbies.Should().HaveCount(1);

            var lobby = lobbies.First();
            lobby.Content.Access.Should().Be(LobbyAccess.Public);
            lobby.GameType.Should().Be(GameType.Chess);
            lobby.Content.Name.Should().Be("Teszt lobby");
        }

        [TestMethod]
        public async Task CannotCreateMultipleLobbies()
        {
            await CreateLobbyAs(LobbyAccess.Public, "teszt1");
            var response = await client.PostJsonAsync(
                "api/lobbies",
                new CreateLobbyDto
                {
                    GameType = GameType.Chess,
                    LobbyAccess = LobbyAccess.Public,
                    Name = "Teszt lobby2"
                },
                "teszt1");

            response.IsSuccessStatusCode.Should().BeFalse();
        }

        [TestMethod]
        public async Task JoinPublicLobbyWorks()
        {
            var resultLobby = await CreateLobbyAs(LobbyAccess.Public, "teszt1");

            var response = await client.PostJsonAsync($"api/lobbies/{resultLobby.Content.Id}/join", null, "teszt2");
            response.IsSuccessStatusCode.Should().BeTrue();
        }

        [TestMethod]
        public async Task JoinPrivateLobbyNotWorksWithoutInvite()
        {
            var resultLobby = await CreateLobbyAs(LobbyAccess.Private, "teszt1");

            var response = await client.PostJsonAsync($"api/lobbies/{resultLobby.Content.Id}/join", null, "teszt2");
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [TestMethod]
        public async Task JoinPrivateLobbyWorksWithInvite()
        {
            var resultLobby = await CreateLobbyAs(LobbyAccess.Private, "teszt1");
            await client.PostJsonAsync($"api/lobbies/{resultLobby.Content.Id}/invite?playerName=teszt2", null, "teszt1");

            var response = await client.PostJsonAsync($"api/lobbies/{resultLobby.Content.Id}/join", null, "teszt2");
            response.IsSuccessStatusCode.Should().BeTrue();
        }

        [TestMethod]
        public async Task JoinFriendsOnlyLobbyNotWorksIfNotFriendsAndNotInvited()
        {
            var resultLobby = await CreateLobbyAs(LobbyAccess.FriendsOnly, "teszt1");

            var response = await client.PostJsonAsync($"api/lobbies/{resultLobby.Content.Id}/join", null, "teszt2");
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [TestMethod]
        public async Task JoinFriendsOnlyLobbyWorksWithInvite()
        {
            var resultLobby = await CreateLobbyAs(LobbyAccess.FriendsOnly, "teszt1");
            await client.PostJsonAsync($"api/lobbies/{resultLobby.Content.Id}/invite?playerName=teszt2", null, "teszt1");

            var response = await client.PostJsonAsync($"api/lobbies/{resultLobby.Content.Id}/join", null, "teszt2");
            response.IsSuccessStatusCode.Should().BeTrue();
        }

        [TestMethod]
        public async Task JoinFriendsOnlyLobbyWorksIfFriends()
        {
            // Setting up friendship between the 2 test users
            await factory.RunWithInjectionAsync(async (CzeumContext context) =>
            {
                var test1 = await context.Users.SingleAsync(u => u.UserName == "teszt1");
                var test2 = await context.Users.SingleAsync(u => u.UserName == "teszt2");

                context.Friendships.Add(new Friendship
                {
                    User1 = test1,
                    User2 = test2
                });

                await context.SaveChangesAsync();
            });

            var resultLobby = await CreateLobbyAs(LobbyAccess.FriendsOnly, "teszt1");

            var response = await client.PostJsonAsync($"api/lobbies/{resultLobby.Content.Id}/join", null, "teszt2");
            response.IsSuccessStatusCode.Should().BeTrue();
        }

        [TestCleanup]
        public void Cleanup()
        {
            client.Dispose();
            factory.Dispose();
        }
    }
}