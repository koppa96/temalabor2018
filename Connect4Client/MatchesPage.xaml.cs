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
using Connect4Dtos;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Connect4Client {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MatchesPage : Page {

        private ObservableCollection<MatchDto> Matches { get { return MatchRepository.Instance.MatchList; } }
        private MatchDto SelectedMatch {
            get { return MatchRepository.Instance.SelectedMatch; }
            set { MatchRepository.Instance.SelectedMatch = value; DrawBoard(); }
        }
       

        private BoardData ParseToBoard(MatchDto match) {
            if(match == null) {
                return null;
            }
            string[] elements = match.BoardData.Split(" ");
            int width = int.Parse(elements[0]), height = int.Parse(elements[1]);

            Item enemyItem = match.YourItem == Item.Yellow ? Item.Red : Item.Yellow;

            BoardData board = new BoardData(width, height);

            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    switch (elements[width * i + j + 2]) {
                        case "1":
                            board.SetItemAt(i, j, Item.Yellow);
                            break;
                        case "2":
                            board.SetItemAt(i, j, Item.Red);
                            break;
                    }
                }
            }
            return board;
        }

        public MatchesPage() {
            this.InitializeComponent();
            MatchRepository.Instance.AddMatchPage(this);
        }
        
        public void DrawBoard() {
            if(SelectedMatch == null) {
                return;
            }
            BoardData board = ParseToBoard(SelectedMatch);
            if (board == null) {
                return;
            }
            int width_px = (int) BoardCanvas.ActualWidth;
            int heigt_px = (int) BoardCanvas.ActualHeight;

            int radius = 20;

            BoardCanvas.Children.Clear();

            for(int i = 0; i < board.Width; i++) {
                for (int j = 0; j < board.Height; j++) {
                    var ellipse = new Ellipse();
                    ellipse.Width = radius;
                    ellipse.Height = radius;
                    ellipse.Tag = i.ToString();
                    ellipse.Stroke = new SolidColorBrush(Windows.UI.Colors.Black);
                    Item item = board.GetItemAt(j, i);
                    ellipse.Fill = new SolidColorBrush(item == Item.Red ? Windows.UI.Colors.Red : item == Item.Yellow ? Windows.UI.Colors.Yellow : Windows.UI.Colors.Gray);
                    Canvas.SetLeft(ellipse, i * (radius + 10));
                    Canvas.SetTop(ellipse, j * (radius + 10));
                    BoardCanvas.Children.Add(ellipse);
                    ellipse.PointerReleased += Ellipse_PointerReleased;
                }
            }
        }

        private void Ellipse_PointerReleased(object sender, PointerRoutedEventArgs e) {
            var ellipse = sender as Ellipse;
            int column = Int32.Parse(ellipse.Tag.ToString());
            ConnectionManager.Instance.PlaceItem(SelectedMatch.MatchId, column);
        }
        
    }
}
