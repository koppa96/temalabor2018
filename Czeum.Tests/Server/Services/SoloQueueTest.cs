using System;
using System.Collections.Generic;
using System.Text;
using Czeum.Server.Services.SoloQueue;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Czeum.Tests.Server.Services
{
    [TestClass]
    public class SoloQueueTest
    {
        private ISoloQueueService service;

        [TestInitialize]
        public void Init()
        {
            service = new SoloQueueService();
        }

        [TestMethod]
        [DataRow("TestPlayer")]
        public void AddPlayerToSoloQueue(string playerName)
        {
            service.JoinSoloQueue(playerName);

            Assert.IsTrue(service.IsQueuing(playerName));
        }

        [TestMethod]
        public void TryToPopWithNotEnoughPlayers()
        {
            var players = service.PopFirstTwoPlayers();
            Assert.AreEqual(null, players);

            service.JoinSoloQueue("TestPlayer");
            players = service.PopFirstTwoPlayers();
            Assert.AreEqual(null, players);
        }

        [TestMethod]
        public void PopWithTwoPlayers()
        {
            service.JoinSoloQueue("TestPlayer");
            service.JoinSoloQueue("OtherPlayer");

            var players = service.PopFirstTwoPlayers();
            Assert.AreNotEqual(null, players);
            Assert.AreEqual(2, players.Length);
            Assert.AreEqual("TestPlayer", players[0]);
            Assert.AreEqual("OtherPlayer", players[1]);

            Assert.IsFalse(service.IsQueuing("TestPlayer"));
            Assert.IsFalse(service.IsQueuing("OtherPlayer"));
        }

        [TestMethod]
        public void PopWithMoreThanTwoPlayers()
        {
            service.JoinSoloQueue("TestPlayer");
            service.JoinSoloQueue("SecondPlayer");
            service.JoinSoloQueue("ThirdPlayer");

            var players = service.PopFirstTwoPlayers();
            Assert.IsTrue(service.IsQueuing("ThirdPlayer"));
        }

        [TestMethod]
        public void LeaveSoloQueueTest()
        {
            service.JoinSoloQueue("TestPlayer");
            Assert.IsTrue(service.IsQueuing("TestPlayer"));
            
            service.LeaveSoloQueue("TestPlayer");
            Assert.IsFalse(service.IsQueuing("TestPlayer"));

            service.JoinSoloQueue("OtherPlayer");
            var players = service.PopFirstTwoPlayers();
            Assert.AreEqual(null, players);
        }
    }
}
