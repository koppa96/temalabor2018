using Czeum.ChessLogic;
using Czeum.ChessLogic.Pieces;
using Czeum.DTO.Chess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Czeum.Tests.ChessLogic
{
    [TestClass]
    public class BoardTest
    {
        private ChessBoard board;

        [TestInitialize]
        public void Init()
        {
            board = new ChessBoard(false);
        }

        [TestMethod]
        public void CheckKingSafety()
        {
            var king = new King(board, Color.White);
            board.AddPieceToTheGame(king, board[4, 4]);
            Assert.IsTrue(board.IsKingSafe(Color.White));
            
            var friend = new Rook(board, Color.White);
            board.AddPieceToTheGame(friend, board[4, 5]);
            Assert.IsTrue(board.IsKingSafe(Color.White));
            
            var enemy = new Rook(board, Color.Black);
            board.AddPieceToTheGame(enemy, board[0, 4]);
            Assert.IsFalse(board.IsKingSafe(Color.White));
        }

        [TestMethod]
        public void PossibleMoveGeneration()
        {
            var king = new King(board, Color.White);
            board.AddPieceToTheGame(king, board[1, 1]);
            
            Assert.AreEqual(8, board.GetPossibleMovesFor(Color.White).Count);
            king.Move(board[0, 1]);
            Assert.AreEqual(5, board.GetPossibleMovesFor(Color.White).Count);
            king.Move(board[0, 0]);
            Assert.AreEqual(3, board.GetPossibleMovesFor(Color.White).Count);
        }

        [TestMethod]
        public void DetectStalemate()
        {
            var king = new King(board, Color.White);
            board.AddPieceToTheGame(king, board[0, 0]);
            
            var enemyRook = new Rook(board, Color.Black);
            board.AddPieceToTheGame(enemyRook, board[6, 1]);
            
            var otherEnemy = new Rook(board, Color.Black);
            board.AddPieceToTheGame(otherEnemy, board[1, 6]);
            
            Assert.IsTrue(board.Stalemate(Color.White, board.GetPossibleMovesFor(Color.White)));
            Assert.IsFalse(board.Checkmate(Color.White, board.GetPossibleMovesFor(Color.White)));
            
            var friend = new Bishop(board, Color.White);
            board.AddPieceToTheGame(friend, board[4, 4]);
            Assert.IsFalse(board.Stalemate(Color.White, board.GetPossibleMovesFor(Color.White)));
        }

        [TestMethod]
        public void DetectCheckmate()
        {
            var king = new King(board, Color.White);
            board.AddPieceToTheGame(king, board[0, 0]);
            
            var enemyRook = new Rook(board, Color.Black);
            board.AddPieceToTheGame(enemyRook, board[7, 0]);
            Assert.IsFalse(board.Checkmate(Color.White, board.GetPossibleMovesFor(Color.White)));
            
            var otherRook = new Rook(board, Color.Black);
            board.AddPieceToTheGame(otherRook, board[7, 1]);
            Assert.IsTrue(board.Checkmate(Color.White, board.GetPossibleMovesFor(Color.White)));
            
            var whiteRook = new Rook(board, Color.White);
            board.AddPieceToTheGame(whiteRook, board[4, 4]);
            Assert.IsFalse(board.Checkmate(Color.White, board.GetPossibleMovesFor(Color.White)));

            whiteRook.Move(board[0, 4]);
            enemyRook.Move(board[0, 4]);
            Assert.IsTrue(board.Checkmate(Color.White, board.GetPossibleMovesFor(Color.White)));
            
            board.AddPieceToTheGame(whiteRook, board[4, 4]);
            Assert.IsFalse(board.Checkmate(Color.White, board.GetPossibleMovesFor(Color.White)));
        }
    }
}