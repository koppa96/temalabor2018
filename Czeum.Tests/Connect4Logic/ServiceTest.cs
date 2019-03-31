using System.Collections.Generic;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.ChessLogic;
using Czeum.Connect4Logic;
using Czeum.DTO.Connect4;
using Czeum.Server.Services.ServiceContainer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Czeum.Tests.Connect4Logic
{
    [TestClass]
    public class ServiceTest
    {
        private Connect4MoveData move;
        private DummyRepository repository;
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

            repository = new DummyRepository();
            originalService = new Connect4Service(repository);
            var services = new List<IGameService>
            {
                new ChessService(null),
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
            var result = (Connect4MoveResult) service.ExecuteMove(move, 1);
            
            Assert.AreEqual(Status.Success, result.Status);
            Assert.AreEqual(Item.Red, result.Board[result.Board.GetLength(0) - 1, 0]);
            Assert.IsTrue(repository.GetByMatchId(1).BoardData.Contains('R'));
        }
    }
}