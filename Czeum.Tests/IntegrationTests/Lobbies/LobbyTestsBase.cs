using Czeum.Core.DTOs.Lobbies;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Enums;
using Czeum.Tests.IntegrationTests.Infrastructure;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Czeum.Tests.IntegrationTests.Lobbies
{
    public abstract class LobbyTestsBase
    {
        protected HttpClient client;
        protected CzeumFactory factory;

        public async Task<LobbyDataWrapper> CreateLobbyAs(LobbyAccess lobbyAccess, string user)
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

        [TestInitialize]
        public async Task SetUpWebHost()
        {
            factory = new CzeumFactory();
            client = factory.CreateClient();
            await factory.SeedUsersAsync();
        }

        [TestCleanup]
        public void Cleanup()
        {
            client.Dispose();
            factory.Dispose();
        }
    }
}
