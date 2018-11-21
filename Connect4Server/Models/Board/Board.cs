using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connect4Dtos;

namespace Connect4Server.Models.Board {
	/// <summary>
	/// Represents a board that contains the items that the players placed
	/// </summary>
	public class Board {
		private Item[,] board;

		public int Width { get; private set; }
		public int Height { get; private set; }

		public bool Full {
			get {
				for (int i = 0; i < Width; i++) {
					if (board[0, i] == Item.None) {
						return false;
					}
				}

				return true;
			}
		}

		/// <summary>
		/// Creates a Board with the desired height and width.
		/// </summary>
		/// <param name="width">The width of the board</param>
		/// <param name="height">The height of the board</param>
		public Board(int width, int height) {
			Width = width;
			Height = height;
			board = new Item[height, width];

			for (int i = 0; i < height; i++) {
				for (int j = 0; j < width; j++) {
					board[i, j] = Item.None;
				}
			}
		}

		/// <summary>
		/// Checks each row if there are at least 4 of the same items next to each other.
		/// </summary>
		/// <param name="item">The item that is checked</param>
		/// <returns>True if there are at least 4 consecutive items in a row from the item</returns>
		private bool HorizontalMatch(Item item) {
			for (int i = 0; i < Height; i++) {
				int count = 0;
				for (int j = 0; j < Width; j++) {
					if (board[i, j] == item) {
						count++;

						if (count == 4) {
							return true;
						}
					} else {
						count = 0;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Checks each column if there are at least 4 of the same items next to each other.
		/// </summary>
		/// <param name="item">The item that is checked</param>
		/// <returns>True if there are at least 4 consecutive items in a column from the item</returns>
		private bool VerticalMatch(Item item) {
			for (int j = 0; j < Width; j++) {
				int count = 0;
				for (int i = 0; i < Height; i++) {
					if (board[i, j] == item) {
						count++;

						if (count == 4) {
							return true;
						}
					} else {
						count = 0;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Checks each diagonal that starts from the left and ends on the right if there are 4 of the item next to each other.
		/// </summary>
		/// <param name="item">The item to be checked</param>
		/// <returns>True if there are at least 4 of the item next to each other</returns>
		private bool LeftDiagonalMatch(Item item) {
			int startRow = Height - 4, startCol = 0;

			while (startCol <= Width - 4) {
				int count = 0;
				for (int i = startRow, j = startCol; i < Height && j < Width; i++, j++) {
					if (board[i, j] == item) {
						count++;

						if (count == 4) {
							return true;
						}
					} else {
						count = 0;
					}
				}

				if (startRow != 0) {
					startRow--;
				} else {
					startCol++;
				}
			}

			return false;
		}

		/// <summary>
		/// Checks each diagonal that starts from the right and ends on the left if there are 4 of the item next to each other.
		/// </summary>
		/// <param name="item">The item to be checked</param>
		/// <returns>True if there are at least 4 of the item next to each other</returns>
		private bool RightDiagonalMatch(Item item) {
			int startRow = 0, startCol = 3;

			while (startRow <= Height - 4) {
				int count = 0;
				for (int i = startRow, j = startCol; i < Height && j >= 0; i++, j--) {
					if (board[i, j] == item) {
						count++;

						if (count == 4) {
							return true;
						}
					} else {
						count = 0;
					}
				}

				if (startCol != Height - 1) {
					startCol++;
				} else {
					startRow++;
				}
			}

			return false;
		}

		/// <summary>
		/// Checks if any player won the game.
		/// </summary>
		/// <returns>The item of the winner player. If no one won yet, it returns the empty item.</returns>
		public Item CheckWinner() {
			foreach (Item item in Enum.GetValues(typeof(Item))) {
				if (item != Item.None && (HorizontalMatch(item) || VerticalMatch(item) || LeftDiagonalMatch(item) || RightDiagonalMatch(item))) {
					return item;
				}
			}

			return Item.None;
		}

		/// <summary>
		/// Tries to place an item in the desired column.
		/// </summary>
		/// <param name="item">The item to be placed</param>
		/// <param name="columnIndex">The index of the desired column</param>
		/// <returns>True if the placing was successful, false if the column is full</returns>
		public bool PutItemToColumn(Item item, int columnIndex) {
			for (int i = Height - 1; i >= 0; i--) {
				if (board[i, columnIndex] == Item.None) {
					board[i, columnIndex] = item;
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Serializes the board into a string that can be stored in a database or sent in a HTTP message.
		/// </summary>
		/// <returns>The serialized board</returns>
		public override string ToString() {
			StringBuilder builder = new StringBuilder(Width.ToString() + " " + Height.ToString() + " ");
			for (int i = 0; i < Height; i++) {
				for (int j = 0; j < Width; j++) {
					switch (board[i, j]) {
						case Item.None:
							builder.Append("0 ");
							break;
						case Item.Yellow:
							builder.Append("1 ");
							break;
						case Item.Red:
							builder.Append("2 ");
							break;
					}
				}
			}

			return builder.ToString();
		}

		/// <summary>
		/// Parses a string and converts it into a board
		/// </summary>
		/// <param name="str">The serialized board</param>
		/// <returns>A new board object that contains the data of the serialized board</returns>
		public static Board Parse(string str) {
			string[] elements = str.Split(" ");
			int width = int.Parse(elements[0]), height = int.Parse(elements[1]);

			Board newBoard = new Board(width, height);

			for (int i = 0; i < height; i++) {
				for (int j = 0; j < width; j++) {
					switch (elements[width * i + height + 2]) {
						case "0":
							newBoard.board[i, j] = Item.None;
							break;
						case "1":
							newBoard.board[i, j] = Item.Yellow;
							break;
						case "2":
							newBoard.board[i, j] = Item.Red;
							break;
					}
				}
			}

			return newBoard;
		}
	}
}
