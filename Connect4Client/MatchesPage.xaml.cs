using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Connect4Client.DTOs;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Connect4Client {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MatchesPage : Page {

        private static int CIRCLE_R = 100;

        private DTOs.BoardData boardData = new DTOs.BoardData(6, 4);

        public MatchesPage() {
            this.InitializeComponent();
            boardData.SetItemAt(3, 2, Item.Red);
            boardData.SetItemAt(2, 2, Item.Red);
            boardData.SetItemAt(3, 3, Item.Yellow);
            DrawBoard(boardData);
        }

        private void DrawBoard(BoardData boardData) {
            for(int i = 0; i < boardData.Width; i++) {
                for (int j = 0; j < boardData.Height; j++) {
                    var ellipse = new Ellipse();
                    ellipse.Width = CIRCLE_R;
                    ellipse.Height = CIRCLE_R;
                    ellipse.Stroke = new SolidColorBrush(Windows.UI.Colors.Black);
                    Item item = boardData.GetItemAt(j, i);
                    ellipse.Fill = new SolidColorBrush(item == Item.Red ? Windows.UI.Colors.Red : item == Item.Yellow ? Windows.UI.Colors.Yellow : Windows.UI.Colors.White);
                    Canvas.SetLeft(ellipse, i * (CIRCLE_R + 10));
                    Canvas.SetTop(ellipse, j * (CIRCLE_R + 10));
                    BoardCanvas.Children.Add(ellipse);
                }
            }
        }
    }
}
