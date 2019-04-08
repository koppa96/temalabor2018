using System.Collections.Generic;
using Czeum.Server.Services.ServiceContainer;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.DAL;
using Czeum.DAL.Entities;
using Czeum.DTO;

namespace Czeum.Server.Services.GameHandler
{
    public class GameHandler : IGameHandler
    {
        private readonly IServiceContainer _serviceContainer;
        private readonly IUnitOfWork _unitOfWork;

        public GameHandler(IServiceContainer serviceContainer, IUnitOfWork unitOfWork)
        {
            _serviceContainer = serviceContainer;
            _unitOfWork = unitOfWork;
        }

        public Dictionary<string, MatchStatus> CreateMatch(LobbyData lobbyData)
        {
            var service = _serviceContainer.FindService(lobbyData);
            var board = (SerializedBoard) service.CreateNewBoard(lobbyData);

            return CreateMatchWithBoard(lobbyData.Host, lobbyData.Guest, board);
        }
        
        public Dictionary<string, MatchStatus> CreateRandomMatch(string player1, string player2)
        {
            var service = _serviceContainer.GetRandomService();
            var board = (SerializedBoard) service.CreateDefaultBoard();

            return CreateMatchWithBoard(player1, player2, board);
        }

        private Dictionary<string, MatchStatus> CreateMatchWithBoard(string player1, string player2, SerializedBoard board)
        {
            _unitOfWork.BoardRepository.InsertBoard(board);
            var match = _unitOfWork.MatchRepository.CreateMatch(player1, player2, board);
            _unitOfWork.Save();
            
            return new Dictionary<string, MatchStatus>
            {
                { player1, match.ToMatchStatus(player1) },
                { player2, match.ToMatchStatus(player2) }
            };
        }

        public Dictionary<string, MatchStatus> HandleMove(MoveData moveData, int playerId)
        {
            var service = _serviceContainer.FindService(moveData);
            var board = _unitOfWork.BoardRepository.GetByMatchId(moveData.MatchId);
            var result = service.ExecuteMove(moveData, playerId, board);
            
            _unitOfWork.BoardRepository.UpdateBoardData(board.BoardId, result.UpdatedBoardData);
            var match = _unitOfWork.MatchRepository.UpdateMatchByStatus(moveData.MatchId, result.MoveResult.Status);
            _unitOfWork.Save();

            var status1 = match.ToMatchStatus(match.Player1.UserName);
            status1.CurrentBoard = result.MoveResult;
            var status2 = match.ToMatchStatus(match.Player2.UserName);
            status2.CurrentBoard = result.MoveResult;
            
            return new Dictionary<string, MatchStatus>
            {
                { match.Player1.UserName, status1 },
                { match.Player2.UserName, status2 }
            };
        }

        public Match GetMatchById(int id)
        {
            return _unitOfWork.MatchRepository.GetMatchById(id);
        }

        public List<MatchStatus> GetMatchesOf(string player)
        {
            return _unitOfWork.MatchRepository.GetMatchesOf(player);
        }

        public MoveResult GetBoardByMatchId(int matchId)
        {
            return _unitOfWork.BoardRepository.GetByMatchId(matchId)?.ToMoveResult();
        }
    }
}