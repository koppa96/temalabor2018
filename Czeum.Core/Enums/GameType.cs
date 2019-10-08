using Czeum.Core.DTOs.Chess;
using Czeum.Core.DTOs.Connect4;
using Czeum.Core.GameServices;

namespace Czeum.Core.Enums
{
    public enum GameType
    {
        [GameType(typeof(ChessLobbyData), typeof(ChessMoveData), typeof(ChessMoveResult))]
        Chess,
        
        [GameType(typeof(Connect4LobbyData), typeof(Connect4MoveData), typeof(Connect4MoveResult))]
        Connect4
    }
}