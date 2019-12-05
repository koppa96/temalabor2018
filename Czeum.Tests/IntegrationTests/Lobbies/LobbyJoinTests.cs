using Czeum.Core.Enums;
using Czeum.DAL;
using Czeum.Domain.Entities;
using Czeum.Tests.IntegrationTests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Czeum.Tests.IntegrationTests.Lobbies
{
    [TestClass]
    public class LobbyJoinTests : LobbyTestsBase
    {
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
    }
}
