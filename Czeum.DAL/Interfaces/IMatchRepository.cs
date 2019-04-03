using System;
using System.Collections.Generic;
using System.Text;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.DAL.Entities;
using Czeum.DTO;

namespace Czeum.DAL.Interfaces
{
    public interface IMatchRepository
    {
        Dictionary<string, MatchStatus> CreateMatchStatuses(int matchId, MoveResult moveResult);
        Dictionary<string, MatchStatus> CreateMatchStatuses(int matchId, int boardId);
        List<MatchStatus> GetMatchesOf(string player);
        Match GetMatchById(int matchId);
        void UpdateMatchByStatus(int matchId, Status status);
        int CreateMatch(LobbyData lobbyData, int boardId);
        int CreateMatch(string player1, string player2, int boardId);
    }
}
