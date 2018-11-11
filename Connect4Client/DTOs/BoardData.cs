using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Client.DTOs {
    class BoardData {
        private Item[,] board;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public BoardData(int width, int height) {
            Width = width;
            Height = height;
            board = new Item[height, width];

            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    board[i, j] = Item.None;
                }
            }
        }
        
        public void SetItemAt(int row, int column, Item i) {
            board[row, column] = i;
        }

        public Item GetItemAt(int row, int column) {
            return board[row, column];
        }
    }
}
