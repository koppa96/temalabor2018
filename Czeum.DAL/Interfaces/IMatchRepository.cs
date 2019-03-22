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
        MatchStatus CreateMatchStatusFor(int matchId, string player, MoveResult moveResult);
        List<Match> GetMatchesOf(string player);
        GameState GetGameStateForMatch(Match match, string player);
        string GetOtherPlayer(int matchId, string player);
        Match GetMatchById(int matchId);
        void UpdateMatchByStatus(int matchId, Status status);
        int CreateMatch(LobbyData lobbyData, int boardId);
    }
}
