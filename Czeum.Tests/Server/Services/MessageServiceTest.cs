using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Czeum.DAL;
using Czeum.DAL.Entities;
using Czeum.DTO.Connect4;
using Czeum.Server.Services.Lobby;
using Czeum.Server.Services.MessageService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Czeum.Tests.Server.Services
{
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
        }

        [TestCleanup]
        public void DisposeContext()
        {
            context.Dispose();
        }
    }
}
