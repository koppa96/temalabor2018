using System;
using Czeum.ChessLogic;
using Czeum.ChessLogic.Pieces;
using Czeum.Core.DTOs.Chess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Czeum.Tests.UnitTests.ChessLogic.PieceTest
{
    [TestClass]
    public class KingTest
    {
        private ChessBoard board;
        private King king;
        
        [TestInitialize]
        public void Init()
        {
            board = new ChessBoard(false);
            king = new King(board, Color.White);
            board.AddPieceToTheGame(king, board[4, 4]);
        }

        [TestMethod]
        public void TestValidMoves()
        {
            Assert.IsTrue(king.Move(board[3, 4]));
            Assert.IsTrue(king.Move(board[2, 3]));
            Assert.IsTrue(king.Move(board[3, 3]));
            Assert.IsTrue(king.Move(board[4, 4]));
        }

        [TestMethod]
        public void TestInvalidMoves()
        {
            for (int i = 0; i < ChessBoard.ChessboardSize; i++)
            {
                for (int j = 0; j < ChessBoard.ChessboardSize; j++)
                {
                    if (Math.Abs(i - 4) < 2 && Math.Abs(j - 4) < 2)
                    {
                        continue;
                    }

                    Assert.IsFalse(king.Move(board[i, j]));
                    Assert.IsTrue(board[i, j].Empty);
                }
            }
        }

        [TestMethod]
        public void TestNotMovingIntoCheck()
        {
            var rook = new Rook(board, Color.White);
            var enemyRook = new Rook(board, Color.Black);
            board.AddPieceToTheGame(rook, board[0, 3]);
            board.AddPieceToTheGame(enemyRook, board[0, 5]);

            Assert.IsFalse(king.Move(board[4, 5]));
            Assert.IsFalse(king.Move(board[5, 5]));
            Assert.IsFalse(king.Move(board[3, 5]));

            Assert.IsTrue(king.Move(board[4, 3]));
            Assert.IsTrue(king.Move(board[3, 3]));
        }

        [TestMethod]
        public void TestKingHittingPiece()
        {
            var enemyRook = new Rook(board, Color.Black);
            board.AddPieceToTheGame(enemyRook, board[3, 4]);

            Assert.IsFalse(king.Move(board[5, 4]));
            Assert.IsTrue(king.Move(board[3, 4]));
        }
    }
}