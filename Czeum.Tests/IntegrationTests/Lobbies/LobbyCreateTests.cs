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
using Czeum.Tests.IntegrationTests.Lobbies;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Czeum.Tests.IntegrationTests
{
    [TestClass]
    public class LobbyCreateTests : LobbyTestsBase
    {        
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
    }
}