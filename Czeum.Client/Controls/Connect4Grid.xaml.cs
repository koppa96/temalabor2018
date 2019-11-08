using Czeum.Client.ViewModels;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Connect4;
using Microsoft.Xaml.Interactions.Core;
using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Czeum.Client.Controls
{
    public sealed partial class Connect4Grid : UserControl
    {
        public Connect4Grid()
        {
            this.InitializeComponent();
        }

        public ICommand MoveCommand {
            get { return (ICommand)GetValue(MoveCommandProperty); }
            set { SetValue(MoveCommandProperty, value); }
        }


        public static readonly DependencyProperty MatchProperty = DependencyProperty.Register(
            "Match",
            typeof(MatchStatus),
            typeof(Connect4Grid),
            new PropertyMetadata(null, new PropertyChangedCallback(OnMatchChanged))
        );
        public static readonly DependencyProperty MoveCommandProperty = DependencyProperty.Register(
            "MoveCommand",
            typeof(ICommand),
            typeof(Connect4Grid),
            new PropertyMetadata(null, new PropertyChangedCallback(OnMoveCommandChanged))
        );

        public MatchStatus Match {
            get { return (MatchStatus)GetValue(MatchProperty); }
            set { SetValue(MatchProperty, value); }
        }


        private static void OnMatchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Connect4Grid).RenderBoard();
        }
        private static void OnMoveCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ;
        }

        private void RenderBoard()
        {
            var boardData = Match.CurrentBoard.Content as Connect4MoveResult;

            if (boardData == null)
            {
                return;
            }
            var board = boardData.Board;

            BoardContainer.Children.Clear();
            BoardContainer.RowDefinitions.Clear();
            BoardContainer.ColumnDefinitions.Clear();

            for (int i = 0; i < board.GetLength(0); i++)
            {
                BoardContainer.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            }
            for (int i = 0; i < board.GetLength(1); i++)
            {
                BoardContainer.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for(int j = 0; j <board.GetLength(1); j++)
                {
                    Ellipse e = new Ellipse() {
                        Fill = new SolidColorBrush(board[i,j] == Item.Red ? Colors.DarkRed : board[i,j] == Item.Yellow ? Colors.Gold : Colors.Gainsboro),
                        Stroke = new SolidColorBrush(Colors.DarkGray), StrokeThickness = 4,
                        Stretch = Stretch.Uniform,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Margin = new Thickness(10, 5, 10, 5),
                        Tag = j,
                    };
                    e.SetValue(Grid.RowProperty, i);
                    e.SetValue(Grid.ColumnProperty, j);
                    var tempj = j;
                    e.Tapped += (sender, args) =>
                    {
                        var moveData = new Connect4MoveData() { Column = tempj, MatchId = Match.Id};
                        MoveCommand.Execute(moveData);
                    };

                    BoardContainer.Children.Add(e);
                }
            }
        }
    }
}
