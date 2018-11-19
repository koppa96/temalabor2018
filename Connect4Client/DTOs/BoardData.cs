using System;
using Connect4Dtos;

namespace Connect4Client {
    public class BoardData {
        private Item[] board;

        public int Width { get; set; }
        public int Height { get; set; }

        public BoardData(int width, int height) {
            Height = height;
            Width = width;
            board = new Item[height * width];

            for (int i = 0; i < Height * Width; i++) {
                board[i] = Item.None;
            }
        }

        public void SetItemAt(int row, int column, Item item) {
            board[Height * column + row] = item;
        }
        public Item GetItemAt(int row, int column) {
            return board[Height * column + row];
        }
    }
}