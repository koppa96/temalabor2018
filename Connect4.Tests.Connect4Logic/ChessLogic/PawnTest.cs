using System;
using System.Collections.Generic;
using System.Text;
using Connect4.ChessLogic;
using Connect4.ChessLogic.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Connect4.Tests.ChessLogic
{
    [TestClass]
    public class PawnTest
    {
        private const int ChessboardSize = 8;
        private ChessBoard board;

        [TestInitialize]
        public void Init()
        {
            board = new ChessBoard(false);
        }

        [TestMethod]
        public void PawnForwardMovementWhite()
        {
            var pawn = new Pawn(board, Color.White);

            pawn.AddToField(board[4, 4]);
            Assert.IsFalse(board[4, 4].Empty);

            Assert.IsTrue(pawn.Move(board[3, 4]));
            Assert.IsFalse(board[3, 4].Empty);
            Assert.IsTrue(board[4, 4].Empty);
        }

        [TestMethod]
        public void PawnWrongMovementsWhite()
        {
            var pawn = new Pawn(board, Color.White);
            pawn.AddToField(board[4, 4]);

            for (int i = 0; i < ChessboardSize; i++)
            {
                for (int j = 0; j < ChessboardSize; j++)
                {
                    if (i != 3 && j != 4)
                    {
                        Assert.IsFalse(pawn.Move(board[i, j]));
                        Assert.IsTrue(board[i, j].Empty);
                    }
                }
            }

            Assert.IsFalse(board[4, 4].Empty);
        }

        [TestMethod]
        public void PawnFirstMoveTest()
        {
            var pawn = new Pawn(board, Color.White);
            pawn.AddToField(board[4, 4]);

            //First double move
            Assert.IsTrue(pawn.Move(board[2, 4]));
            Assert.IsFalse(board[2, 4].Empty);
            Assert.IsTrue(board[4, 4].Empty);

            //Second double move
            Assert.IsFalse(pawn.Move(board[0, 4]));
            Assert.IsFalse(board[2, 4].Empty);
            Assert.IsTrue(board[0, 4].Empty);

            //Try simple move
            Assert.IsTrue(pawn.Move(board[1, 4]));
            Assert.IsFalse(board[1, 4].Empty);
            Assert.IsTrue(board[2, 4].Empty);
        }

        [TestMethod]
        public void PawnHits()
        {
            var pawn = new Pawn(board, Color.White);
            pawn.AddToField(board[4, 4]);

            var enemy = new Pawn(board, Color.Black);
            enemy.AddToField(board[3, 4]);

            var otherEnemy = new Pawn(board, Color.Black);
            otherEnemy.AddToField(board[3, 3]);

            var friend = new Pawn(board, Color.White);
            friend.AddToField(board[3, 5]);

            Assert.IsFalse(pawn.Move(board[3, 5]));
            Assert.IsFalse(pawn.Move(board[3, 4]));
            Assert.IsTrue(pawn.Move(board[3, 3]));
        }

        [TestMethod]
        public void PawnTriesToStepOver()
        {
            var pawn = new Pawn(board, Color.White);
            pawn.AddToField(board[4, 4]);

            var other = new Pawn(board, Color.White);
            other.AddToField(board[3, 4]);

            Assert.IsFalse(pawn.Move(board[2, 4]));
        }
    }
}
