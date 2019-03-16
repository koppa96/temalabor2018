using System;
using System.Collections.Generic;
using System.Text;
using Connect4.ChessLogic;
using Connect4.ChessLogic.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Connect4.Tests.ChessLogic
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
        public void TestRouteClear()
        {
            var pawn = new Pawn(board, Color.White);
            pawn.AddToField(board[4, 4]);

            Assert.IsTrue(board.RouteClear(board[0, 0], board[0, 1]));
            Assert.IsTrue(board.RouteClear(board[0, 0], board[0, 6]));
        }
    }
}
