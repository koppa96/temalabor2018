using System;
using System.Collections.Generic;
using System.Linq;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.ChessLogic;
using Czeum.Connect4Logic;
using Czeum.DTO.Chess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Czeum.Tests.ChessLogic
{
    [TestClass]
    public class ServiceTest
    {
        private ChessMoveData move;
        private DummyRepository repository;
        private List<IGameService> services;

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
            services = new List<IGameService>
            {
                new Connect4Service(null),
                new ChessService(repository)
            };
        }

        [TestMethod]
        public void MoveDataFindsService()
        {
            var service = move.FindGameService(services);
            Assert.AreSame(services[1], service);
        }

        [TestMethod]
        public void TestExecute()
        {
            var service = move.FindGameService(services);
            var result = (ChessMoveResult) service.ExecuteMove(move, 1);
            
            Assert.AreEqual(Status.Success, result.Status);
            Assert.IsTrue(result.PieceInfos.Any(p => p.Row == move.ToRow && p.Column == move.ToColumn));

            var pieceInfos = repository.GetById(1).BoardData.Trim().Split(' ');
            Assert.IsTrue(pieceInfos.Contains($"WP_{move.ToRow},{move.ToColumn}"));
        }
    }
}