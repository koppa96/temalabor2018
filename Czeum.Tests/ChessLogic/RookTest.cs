using System;
using System.Collections.Generic;
using System.Text;
using Czeum.ChessLogic;
using Czeum.ChessLogic.Pieces;
using Czeum.DTO.Chess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Czeum.Tests.ChessLogic
{
    [TestClass]
    public class RookTest
    {
        private ChessBoard board;

        [TestInitialize]
        public void Init()
        {
            board = new ChessBoard(false);
        }

        [TestMethod]
        public void TestValidMoves()
        {
            var rook = new Rook(board, Color.White);
            board.AddPieceToTheGame(rook, board[4, 4]);

            Assert.IsTrue(rook.Move(board[7, 4]));
            Assert.IsTrue(board[4, 4].Empty);
            Assert.IsFalse(board[7, 4].Empty);

            Assert.IsTrue(rook.Move(board[7, 7]));
            Assert.IsTrue(rook.Move(board[0, 7]));
            Assert.IsTrue(rook.Move(board[0, 0]));
            Assert.IsTrue(rook.Move(board[0, 1]));
        }

        [TestMethod]
        public void TestInvalidMoves()
        {
            var rook = new Rook(board, Color.White);
            board.AddPieceToTheGame(rook, board[4, 4]);

            for (int i = 0; i < ChessBoard.ChessboardSize; i++)
            {
                for (int j = 0; j < ChessBoard.ChessboardSize; j++)
                {
                    if (i != 4 && j != 4)
                    {
                        Assert.IsFalse(rook.Move(board[i, j]));
                        Assert.IsTrue(board[i, j].Empty);
                    }
                }
            }

            Assert.IsFalse(board[4, 4].Empty);

            var otherRook = new Rook(board, Color.Black);
            board.AddPieceToTheGame(otherRook, board[4, 6]);

            Assert.IsFalse(rook.Move(board[4, 7]));
            Assert.IsTrue(rook.Move(board[4, 6]));
        }
    }
}
