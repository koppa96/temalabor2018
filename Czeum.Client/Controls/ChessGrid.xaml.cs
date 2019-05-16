using Czeum.Client.ViewModels;
using Czeum.DTO;
using Czeum.DTO.Chess;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Czeum.Client.Controls
{
    public sealed partial class ChessGrid : UserControl
    {
        public ChessGrid()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty MatchProperty = DependencyProperty.Register(
            "Match",
            typeof(MatchStatus),
            typeof(ChessGrid),
            new PropertyMetadata(null, new PropertyChangedCallback(OnMatchChanged))
        );

        public MatchStatus Match {
            get { return (MatchStatus)GetValue(MatchProperty); }
            set { SetValue(MatchProperty, value); }
        }

        private static void OnMatchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChessGrid).RenderBoard();
        }

        private void RenderBoard()
        {
            var boardData = Match.CurrentBoard as ChessMoveResult;

            if (boardData == null)
            {
                return;
            }

            BoardContainer.Children.Clear();
            if(Match.PlayerId == 1)
            {
                DrawBoard();
                PlacePieces(boardData);
            }
            else
            {
                DrawBoardFlipped();
                PlacePiecesFlipped(boardData);
            }
        }

        private void PlacePieces(ChessMoveResult boardData)
        {
            foreach (var piece in boardData.PieceInfos)
            {
                Image i = new Image();
                i.Stretch = Stretch.Uniform;
                i.HorizontalAlignment = HorizontalAlignment.Center;
                i.VerticalAlignment = VerticalAlignment.Center;
                string colorChar = piece.Color == DTO.Chess.Color.White ? "w" : "b";
                string typeChar = GetPieceTypeString(piece);
                string imagePath = $"{colorChar}_{piece.Type.ToString().ToLower()}_svg_withShadow.png";
                i.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{imagePath}"));
                i.SetValue(Grid.RowProperty, piece.Row);
                i.SetValue(Grid.ColumnProperty, piece.Column);

                var action = new InvokeCommandAction() { Command = (DataContext as ChessPageViewModel)?.PieceSelectedCommand, CommandParameter = new Tuple<int, int>(piece.Column, piece.Row) };
                var behavior = new EventTriggerBehavior() { EventName = "Tapped" };
                behavior.Actions.Add(action);
                var bColl = new BehaviorCollection();
                bColl.Add(behavior);
                Interaction.SetBehaviors(i, bColl);

                BoardContainer.Children.Add(i);
            }
        }
        private void PlacePiecesFlipped(ChessMoveResult boardData)
        {
            foreach (var piece in boardData.PieceInfos)
            {
                Image i = new Image();
                i.Stretch = Stretch.Uniform;
                i.HorizontalAlignment = HorizontalAlignment.Center;
                i.VerticalAlignment = VerticalAlignment.Center;
                string colorChar = piece.Color == DTO.Chess.Color.White ? "w" : "b";
                string typeChar = GetPieceTypeString(piece);
                string imagePath = $"{colorChar}_{piece.Type.ToString().ToLower()}_svg_withShadow.png";
                i.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{imagePath}"));
                i.SetValue(Grid.RowProperty, 7 - piece.Row);
                i.SetValue(Grid.ColumnProperty, 7 - piece.Column);

                var action = new InvokeCommandAction() { Command = (DataContext as ChessPageViewModel)?.PieceSelectedCommand, CommandParameter = new Tuple<int, int>(piece.Column, piece.Row) };
                var behavior = new EventTriggerBehavior() { EventName = "Tapped" };
                behavior.Actions.Add(action);
                var bColl = new BehaviorCollection();
                bColl.Add(behavior);
                Interaction.SetBehaviors(i, bColl);

                BoardContainer.Children.Add(i);
            }
        }

        private void DrawBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var field = new Rectangle
                    {
                        Fill = new SolidColorBrush((i + j) % 2 == 0 ? Colors.White : Colors.DarkGray),
                        Width = 100,
                        Height = 100,
                        Stretch = Stretch.Uniform
                    };
                    field.SetValue(Grid.RowProperty, i);
                    field.SetValue(Grid.ColumnProperty, j);

                    var action = new InvokeCommandAction() { Command = (DataContext as ChessPageViewModel)?.FieldSelectedCommand, CommandParameter = new Tuple<int, int>(j, i) };
                    var behavior = new EventTriggerBehavior() { EventName = "Tapped" };
                    behavior.Actions.Add(action);
                    var bColl = new BehaviorCollection();
                    bColl.Add(behavior);
                    Interaction.SetBehaviors(field, bColl);

                    BoardContainer.Children.Add(field);
                }
            }
        }
        private void DrawBoardFlipped()
        {
            for (int i = 7; i >= 0; i--)
            {
                for (int j = 7; j >= 0; j--)
                {
                    var field = new Rectangle
                    {
                        Fill = new SolidColorBrush((i + j) % 2 == 0 ? Colors.White : Colors.DarkGray),
                        Width = 100,
                        Height = 100,
                        Stretch = Stretch.Uniform
                    };
                    field.SetValue(Grid.RowProperty, i);
                    field.SetValue(Grid.ColumnProperty, j);

                    var action = new InvokeCommandAction() { Command = (DataContext as ChessPageViewModel)?.FieldSelectedCommand, CommandParameter = new Tuple<int, int>(7 - j, 7 - i) };
                    var behavior = new EventTriggerBehavior() { EventName = "Tapped" };
                    behavior.Actions.Add(action);
                    var bColl = new BehaviorCollection();
                    bColl.Add(behavior);
                    Interaction.SetBehaviors(field, bColl);

                    BoardContainer.Children.Add(field);
                }
            }
        }

        private string GetPieceTypeString(PieceInfo piece)
        {
            switch (piece.Type)
            {
                case PieceType.King:
                    return "k";
                case PieceType.Queen:
                    return "q";
                case PieceType.Bishop:
                    return "b";
                case PieceType.Knight:
                    return "n";
                case PieceType.Rook:
                    return "r";
                case PieceType.Pawn:
                    return "p";
                default:
                    return "p";
            }
        }
    }
}
