using Czeum.Abstractions.DTO.Lobbies;
using Czeum.DTO.Chess;
using Czeum.DTO.Connect4;

namespace Czeum.DTO.Lobbies
{
    public enum LobbyType
    {
        [LobbyType(typeof(ChessLobbyData))]
        Chess,
        
        [LobbyType(typeof(Connect4LobbyData))]
        Connect4
    }
}