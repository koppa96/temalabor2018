using System;
using System.Collections.Generic;
using System.Text;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.ChessLogic;
using Czeum.Connect4Logic;
using Czeum.DAL.Entities;
using Czeum.DTO.Chess;
using Czeum.DTO.Connect4;
using Czeum.Server.Services.ServiceContainer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Czeum.Tests.Server.Services
{
    [TestClass]
    public class ServiceContainerTest
    {
        private IServiceContainer container;

        [TestInitialize]
        public void Init()
        {
            var serviceList = new List<IGameService>
            {
                new ChessService(),
                new Connect4Service()
            };

            container = new ServiceContainer(serviceList);
        }

        [TestMethod]
        public void TestFindByMoveData()
        {
            MoveData moveData = new ChessMoveData();
            var service = container.FindByMoveData(moveData);

            Assert.AreEqual(typeof(ChessService), service.GetType());

            moveData = new Connect4MoveData();
            service = container.FindByMoveData(moveData);

            Assert.AreEqual(typeof(Connect4Service), service.GetType());
        }

        [TestMethod]
        public void TestFindByLobbyData()
        {
            LobbyData lobbyData = new ChessLobbyData();
            var service = container.FindByLobbyData(lobbyData);

            Assert.AreEqual(typeof(ChessService), service.GetType());

            lobbyData = new Connect4LobbyData();
            service = container.FindByLobbyData(lobbyData);

            Assert.AreEqual(typeof(Connect4Service), service.GetType());
        }

        [TestMethod]
        public void TestFindByBoard()
        {
            SerializedBoard board = new SerializedChessBoard();
            var service = container.FindBySerializedBoard(board);

            Assert.AreEqual(typeof(ChessService), service.GetType());

            board = new SerializedConnect4Board();
            service = container.FindBySerializedBoard(board);

            Assert.AreEqual(typeof(Connect4Service), service.GetType());
        }
    }
}
