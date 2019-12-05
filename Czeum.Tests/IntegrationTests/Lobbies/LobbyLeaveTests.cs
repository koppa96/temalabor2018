using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Enums;
using Czeum.Tests.IntegrationTests.Infrastructure;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czeum.Tests.IntegrationTests.Lobbies
{
    [TestClass]
    public class LobbyLeaveTests : LobbyTestsBase
    {
        [TestMethod]
        public async Task LobbyDeletedAfterLeave()
        {
            await CreateLobbyAs(LobbyAccess.Public, "teszt1");

            var response = await client.PostJsonAsync($"api/lobbies/current/leave", null, "teszt1");
            response.IsSuccessStatusCode.Should().BeTrue();

            var lobbies = await client.GetJsonAsync<IEnumerable<LobbyDataWrapper>>("api/lobbies", "teszt1");
            lobbies.Should().BeNullOrEmpty();
        }

        [TestMethod]
        public async Task LobbyHostChangedAfterLeave()
        {
            var resultLobby = await CreateLobbyAs(LobbyAccess.Public, "teszt1");

            await client.PostJsonAsync($"api/lobbies/{resultLobby.Content.Id}/join", null, "teszt2");

            var leaveResponse = await client.PostJsonAsync($"api/lobbies/current/leave", null, "teszt1");
            leaveResponse.IsSuccessStatusCode.Should().BeTrue();

            var lobbies = await client.GetJsonAsync<IEnumerable<LobbyDataWrapper>>("api/lobbies", "teszt1");
            lobbies.Should().HaveCount(1);

            var lobby = lobbies.First();
            lobby.Content.Host.Should().Be("teszt2");
            lobby.Content.Guests.Should().BeNullOrEmpty();
        }
    }
}
