using System;
using System.Collections.Generic;
using System.Text;
using Connect4.ChessLogic;
using Connect4.ChessLogic.Pieces;
using Czeum.DTO.Chess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Connect4.Tests.ChessLogic
{
    [TestClass]
    public class KnightTest
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
            var knight = new Knight(board, Color.White);
            board.AddPieceToTheGame(knight, board[4, 4]);

            string str = knight.ToString();

            Assert.IsTrue(knight.Move(board[3, 6]));
            Assert.IsTrue(knight.Move(board[5, 5]));
            Assert.IsTrue(knight.Move(board[7, 6]));
        }
        
        [TestMethod]
        public void TestInvalidMoves()
        {
            var knight = new Knight(board, Color.White);
            board.AddPieceToTheGame(knight, board[4, 4]);

            for (int i = 0; i < ChessBoard.ChessboardSize; i++)
            {
                for (int j = 0; j < ChessBoard.ChessboardSize; j++)
                {
                    if (Math.Abs(i - 4) == 2 && Math.Abs(j - 4) == 1 || Math.Abs(i - 4) == 1 && Math.Abs(j - 4) == 2)
                    {
                        continue;
                    }

                    Assert.IsFalse(knight.Move(board[i, j]));
                }
            }
        }
    }
}
