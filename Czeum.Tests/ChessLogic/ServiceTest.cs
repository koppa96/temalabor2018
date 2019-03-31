using System;
using System.Collections.Generic;
using System.Linq;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.ChessLogic;
using Czeum.Connect4Logic;
using Czeum.DTO.Chess;
using Czeum.Server.Services.ServiceContainer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Czeum.Tests.ChessLogic
{
    [TestClass]
    public class ServiceTest
    {
        private ChessMoveData move;
        private DummyRepository repository;
        private IServiceContainer serviceContainer;
        private ChessService originalService;

        [TestInitialize]
        public void Init()
        {
            move = new ChessMoveData
            {
                FromRow = 6,
                FromColumn = 0,
                ToRow = 4,
                ToColumn = 0,
                MatchId = 1
            };
            
            repository = new DummyRepository();
            originalService = new ChessService(repository);
            var services = new List<IGameService>
            {
                new Connect4Service(null),
                originalService
            };
            serviceContainer = new ServiceContainer(services);
        }

        [TestMethod]
        public void MoveDataFindsService()
        {
            var foundService = serviceContainer.FindService(move);
            Assert.AreSame(originalService, foundService);
        }

        [TestMethod]
        public void TestExecute()
        {
            var service = serviceContainer.FindService(move);
            var result = (ChessMoveResult) service.ExecuteMove(move, 1);
            
            Assert.AreEqual(Status.Success, result.Status);
            Assert.IsTrue(result.PieceInfos.Any(p => p.Row == move.ToRow && p.Column == move.ToColumn));

            var pieceInfos = repository.GetById(1).BoardData.Trim().Split(' ');
            Assert.IsTrue(pieceInfos.Contains($"WP_{move.ToRow},{move.ToColumn}_t"));
        }
    }
}