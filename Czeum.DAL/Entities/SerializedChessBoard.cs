using System;
using System.Collections.Generic;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.DTO.Chess;

namespace Czeum.DAL.Entities
{
    public class SerializedChessBoard : SerializedBoard
    {
        public override MoveResult ToMoveResult()
        {
            var moveResult = new ChessMoveResult
            {
                Status = Status.Requested,
                PieceInfos = new List<PieceInfo>()
            };

            var pieceInfos = BoardData.Trim().Split(' ');
            foreach (var pieceInfo in pieceInfos)
            {
                var pieceDetails = pieceInfo.Split('_');
                var position = pieceDetails[1].Split(',');
                var info = new PieceInfo
                {
                    Color = pieceDetails[0][0] == 'W' ? Color.White : Color.Black,
                    Row = int.Parse(position[0]),
                    Column = int.Parse(position[1])
                };

                switch (pieceDetails[0][1])
                {
                    case 'P':
                        info.Type = PieceType.Pawn;
                        break;
                    case 'R':
                        info.Type = PieceType.Rook;
                        break;
                    case 'H':
                        info.Type = PieceType.Knight;
                        break;
                    case 'B':
                        info.Type = PieceType.Bishop;
                        break;
                    case 'Q':
                        info.Type = PieceType.Queen;
                        break;
                    case 'K':
                        info.Type = PieceType.King;
                        break;
                    default:
                        throw new FormatException($"Error while parsing the board data: {BoardData}");
                }

                moveResult.PieceInfos.Add(info);
            }

            return moveResult;
        }
    }
}
