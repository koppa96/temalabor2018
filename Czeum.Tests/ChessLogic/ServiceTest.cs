using System;
using System.Collections.Generic;
using System.Linq;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.ChessLogic;
using Czeum.Connect4Logic;
using Czeum.DAL.Entities;
using Czeum.DTO.Chess;
using Czeum.Server.Services.ServiceContainer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Czeum.Tests.ChessLogic
{
    [TestClass]
    public class ServiceTest
    {
        private ChessMoveData move;
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
            
            originalService = new ChessService();
            var services = new List<IGameService>
            {
                new Connect4Service(),
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
            var result = service.ExecuteMove(move, 1, new ChessBoard(true).SerializeContent());
            
            Assert.AreEqual(Status.Success, result.MoveResult.Status);

            var chessResult = (ChessMoveResult) result.MoveResult;
            Assert.IsTrue(chessResult.PieceInfos.Any(p => p.Row == move.ToRow && p.Column == move.ToColumn));

            var pieceInfos = result.UpdatedBoardData.Trim().Split(' ');
            Assert.IsTrue(pieceInfos.Contains($"WP_{move.ToRow},{move.ToColumn}_t"));
        }
    }
}