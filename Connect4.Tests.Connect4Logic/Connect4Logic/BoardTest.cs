using System;
using Connect4.Connect4Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Connect4.Tests.Connect4Logic
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
        public void ColumnFullCheckTest()
        {
            bool result;

            for (int i = 0; i < Height; i++)
            {
                result = board.PlaceItem(Item.Red, 0);
                Assert.IsTrue(result);
            }

            result = board.PlaceItem(Item.Red, 0);
            Assert.IsFalse(result);
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
