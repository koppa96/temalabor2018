using Czeum.Client.ViewModels;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Connect4;
using Microsoft.Xaml.Interactions.Core;
using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        public static readonly DependencyProperty MatchProperty = DependencyProperty.Register(
            "Match",
            typeof(MatchStatus),
            typeof(Connect4Grid),
            new PropertyMetadata(null, new PropertyChangedCallback(OnMatchChanged))
        );

        public MatchStatus Match {
            get { return (MatchStatus)GetValue(MatchProperty); }
            set { SetValue(MatchProperty, value); }
        }

        private static void OnMatchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Connect4Grid).RenderBoard();
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
                        Stroke = new SolidColorBrush(Colors.DarkGray), StrokeThickness = 4
                    };
                    e.Stretch = Stretch.Uniform;
                    //e.HorizontalAlignment = HorizontalAlignment.Stretch;
                    //e.VerticalAlignment = VerticalAlignment.Stretch;
                    e.Margin = new Thickness(10, 5, 10, 5);
                    e.SetValue(Grid.RowProperty, i);
                    e.SetValue(Grid.ColumnProperty, j);

                    var action = new InvokeCommandAction() { Command = (DataContext as Connect4PageViewModel)?.ObjectPlacedCommand, CommandParameter = new Tuple<int, int>(i, j) };
                    var behavior = new EventTriggerBehavior() { EventName = "Tapped"};
                    behavior.Actions.Add(action);
                    var bColl = new BehaviorCollection();
                    bColl.Add(behavior);
                    Interaction.SetBehaviors(e, bColl);

                    BoardContainer.Children.Add(e);
                }
            }
            ;
        }


    }
}
