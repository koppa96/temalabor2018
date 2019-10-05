using Czeum.Abstractions;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.DTO.Chess;
using Czeum.DTO.Connect4;

namespace Czeum.DTO
{
    public enum GameType
    {
        [GameType(typeof(ChessLobbyData), typeof(ChessMoveData), typeof(ChessMoveResult))]
        Chess,
        
        [GameType(typeof(Connect4LobbyData), typeof(Connect4MoveData), typeof(Connect4MoveResult))]
        Connect4
    }
}