using System;
using Czeum.Connect4Logic;
using Czeum.Core.DTOs.Connect4;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Czeum.Tests.UnitTests.Connect4Logic
{
    [TestClass]
    public class BoardTest
    {
        private Connect4Board board;
        private const int Width = 7, Height = 6;

        [TestInitialize]
        public void Init()
        {
            board = new Connect4Board(Width, Height);
        }

        [TestMethod]
        public void TestElementPlacement()
        {
            board.PlaceItem(Item.Red, 0);
            Assert.IsTrue(board.Board[board.Height - 1, 0] == Item.Red);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ColumnFullCheckTest()
        {
            for (int i = 0; i < Height; i++)
            {
                board.PlaceItem(Item.Red, 0);
            }

            board.PlaceItem(Item.Red, 0);
        }

        [TestMethod]
        public void BoardFullTest()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Assert.IsFalse(board.Full);
                    board.PlaceItem(Item.Red, i);
                }
            }

            Assert.IsTrue(board.Full);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WrongColumnIndexCheck()
        {
            board.PlaceItem(Item.Red, Width);
        }

        [TestMethod]
        public void HorizontalWinnerCheck()
        {
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(Item.None, board.CheckWinner());
                board.PlaceItem(Item.Red, i);
            }

            Assert.AreEqual(Item.Red, board.CheckWinner());
        }

        [TestMethod]
        public void VerticalWinnerCheck()
        {
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(Item.None, board.CheckWinner());
                board.PlaceItem(Item.Red, 0);
            }

            Assert.AreEqual(Item.Red, board.CheckWinner());
        }

        [TestMethod]
        public void LeftDiagonalWinnerCheck()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3 - i; j++)
                {
                    Assert.AreEqual(Item.None, board.CheckWinner());
                    board.PlaceItem(Item.Yellow, i);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(Item.None, board.CheckWinner());
                board.PlaceItem(Item.Red, i);
            }

            Assert.AreEqual(Item.Red, board.CheckWinner());
        }

        [TestMethod]
        public void RightDiagonalWinnerCheck()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    Assert.AreEqual(Item.None, board.CheckWinner());
                    board.PlaceItem(Item.Yellow, i);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(Item.None, board.CheckWinner());
                board.PlaceItem(Item.Red, i);
            }

            Assert.AreEqual(Item.Red, board.CheckWinner());
        }
    }
}
