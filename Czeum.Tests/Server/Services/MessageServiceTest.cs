using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.DAL;
using Czeum.DAL.Entities;
using Czeum.DTO.Connect4;
using Czeum.Server.Services.Lobby;
using Czeum.Server.Services.MessageService;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Czeum.Tests.Server.Services
{
    [TestClass]
    public class MessageServiceTest
    {
        private IMessageService service;
        private ApplicationDbContext context;
        private ILobbyStorage storage;

        [TestInitialize]
        public void Init()
        {
            context = new InMemoryDbContextFactory().CreateContext();
            storage = new LobbyStorage();
            service = new MessageService(context, storage);
        }

        [TestMethod]
        [DataRow("TestUser", "Test message!")]
        public void TestSendLobbyMessage(string username, string message)
        {
            var lobby = new Connect4LobbyData();
            lobby.Host = username;
            storage.AddLobby(lobby);

            var result = service.SendToLobby(lobby.LobbyId, message, username);
            
            Assert.AreNotEqual(null, result);
            Assert.AreEqual(username, result.Sender);
            Assert.AreEqual(message, result.Text);
            Assert.AreEqual(1, storage.GetMessages(lobby.LobbyId).Count);
            Assert.AreEqual(message, storage.GetMessages(lobby.LobbyId)[0].Text);
        }

        [TestMethod]
        [DataRow("TestUser", "OtherUser", "Test message!")]
        public async Task TestSendMatchMessageAsync(string username, string otherUser, string message)
        {
            var user = new ApplicationUser { UserName = username };
            var secondUser = new ApplicationUser { UserName = otherUser };
            var match = new Match { Player1 = user, Player2 = secondUser, State = MatchState.Player1Moves };

            context.Users.Add(user);
            context.Users.Add(secondUser);
            context.Matches.Add(match);
            await context.SaveChangesAsync();

            var result = await service.SendToMatchAsync(match.MatchId, message, username);

            Assert.AreNotEqual(null, result);
            Assert.AreEqual(username, result.Sender);
            Assert.AreEqual(message, result.Text);
            Assert.AreEqual(1, match.Messages.Count);
            Assert.AreEqual(message, (await context.Messages.SingleAsync(m => m.Match.MatchId == match.MatchId)).Text);
        }

        [TestCleanup]
        public void DisposeContext()
        {
            context.Dispose();
        }
    }
}
