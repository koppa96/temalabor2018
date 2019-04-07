using System.Collections.Generic;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.ChessLogic;
using Czeum.Connect4Logic;
using Czeum.DAL.Entities;
using Czeum.DTO.Connect4;
using Czeum.Server.Services.ServiceContainer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Czeum.Tests.Connect4Logic
{
    [TestClass]
    public class ServiceTest
    {
        private Connect4MoveData move;
        private Connect4Service originalService;
        private IServiceContainer serviceContainer;

        [TestInitialize]
        public void Init()
        {
            move = new Connect4MoveData
            {
                Column = 0,
                MatchId = 1
            };

            originalService = new Connect4Service();
            var services = new List<IGameService>
            {
                new ChessService(),
                originalService
            };
            serviceContainer = new ServiceContainer(services);
        }

        [TestMethod]
        public void MoveDataFindsService()
        {
            var service = serviceContainer.FindService(move);
            Assert.AreSame(originalService, service);
        }

        [TestMethod]
        public void ExecutionTest()
        {
            var service = serviceContainer.FindService(move);
            var result = service.ExecuteMove(move, 1, new Connect4Board().SerializeContent());
            
            Assert.AreEqual(Status.Success, result.MoveResult.Status);

            var connect4Result = (Connect4MoveResult) result.MoveResult;
            Assert.AreEqual(Item.Red, connect4Result.Board[connect4Result.Board.GetLength(0) - 1, 0]);
            Assert.IsTrue(result.UpdatedBoardData.Contains('R'));
        }
    }
}