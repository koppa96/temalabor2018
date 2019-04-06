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
        List<MatchStatus> GetMatchesOf(string player);
        Match GetMatchById(int matchId);
        Match UpdateMatchByStatus(int matchId, Status status);
        Match CreateMatch(LobbyData lobbyData, SerializedBoard board);
        Match CreateMatch(string player1, string player2, SerializedBoard board);
    }
}
