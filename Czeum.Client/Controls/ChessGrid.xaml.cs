using Czeum.Client.ViewModels;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Chess;
using Microsoft.Xaml.Interactions.Core;
using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Czeum.Client.Controls
{
    public sealed partial class ChessGrid : UserControl
    {
        private class Field
        {
            public int Column;
            public int Row;
        }
        private Field lastTapped = null;
        private Rectangle lastTappedRect = null;
        private Windows.UI.Color lastTappedRectColor = Colors.Transparent;

        public ChessGrid()
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
            typeof(ChessGrid),
            new PropertyMetadata(null, new PropertyChangedCallback(OnMatchChanged))
        );
        public static readonly DependencyProperty MoveCommandProperty = DependencyProperty.Register(
            "MoveCommand",
            typeof(ICommand),
            typeof(ChessGrid),
            new PropertyMetadata(null, new PropertyChangedCallback(OnMoveCommandChanged))
        );


        public MatchStatus Match {
            get { return (MatchStatus)GetValue(MatchProperty); }
            set { SetValue(MatchProperty, value); }
        }

        private static void OnMatchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChessGrid).RenderBoard();
        }

        private static void OnMoveCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ;
        }

        private void RenderBoard()
        {
            var boardData = Match.CurrentBoard.Content as ChessMoveResult;

            if (boardData == null)
            {
                return;
            }

            BoardContainer.Children.Clear();
            bool isFlipped = Match.PlayerIndex != 0;
            DrawBoard(isFlipped);
            PlacePieces(boardData, isFlipped);
        }
        
        private void PlacePieces(ChessMoveResult boardData, bool flipped)
        {
            foreach (var piece in boardData.PieceInfos)
            {
                string colorChar = piece.Color == Core.DTOs.Chess.Color.White ? "w" : "b";
                string typeChar = GetPieceTypeString(piece);
                string imagePath = $"{colorChar}_{piece.Type.ToString().ToLower()}_svg_withShadow.png";

                Image i = new Image
                {
                    Stretch = Stretch.Uniform,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Source = new BitmapImage(new Uri($"ms-appx:///Assets/{imagePath}"))
                };


                int row = !flipped ? piece.Row : 7 - piece.Row;
                int column = !flipped ? piece.Column : 7 - piece.Column;
                i.SetValue(Grid.RowProperty, row);
                i.SetValue(Grid.ColumnProperty, column);
                i.IsHitTestVisible = false;

                BoardContainer.Children.Add(i);
            }
        }

        private void DrawBoard(bool flipped)
        {

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var fieldColor = (i + j) % 2 == 0 ? Colors.White : Colors.DarkGray;
                    var field = new Rectangle
                    {
                        Fill = new SolidColorBrush(fieldColor),
                        Width = 100,
                        Height = 100,
                        Stretch = Stretch.Uniform
                    };
                    field.SetValue(Grid.RowProperty, i);
                    field.SetValue(Grid.ColumnProperty, j);

                    int column = !flipped ? j : 7 - j;
                    int row = !flipped ? i : 7 - i;
                    field.Tapped += (sender, args) =>
                    {
                        HandleTap(row, column, field as Rectangle);
                    };
                    BoardContainer.Children.Add(field);
                }
            }
        }

        private void HandleTap(int row, int column, Rectangle field)
        {
            if(Match.State != Core.Enums.GameState.YourTurn) { return; }
            
            var board = Match.CurrentBoard.Content as ChessMoveResult;
            var playerIndex = Match.PlayerIndex;
            var clickedPiece = board.PieceInfos.FirstOrDefault(p => p.Row == row && p.Column == column);
            // We clicked on an empty field
            if(clickedPiece == null) { 
                // ... and haven't selected any pieces to move, so we return early
                if(lastTapped == null)
                {
                    return;
                }
            }
            // If we clicked on our own piece
            else if ((playerIndex == 0 && clickedPiece.Color == Core.DTOs.Chess.Color.White) || (playerIndex == 1 && clickedPiece.Color == Core.DTOs.Chess.Color.Black))
            {
                // Save the position as our selected piece and return
                lastTapped = new Field() { Column = column, Row = row };
                if (lastTappedRect != null)
                {
                    lastTappedRect.Fill = new SolidColorBrush(lastTappedRectColor);
                }
                lastTappedRect = field;
                var fieldColor = (row + column) % 2 == 0 ? Colors.White : Colors.DarkGray;
                lastTappedRectColor = fieldColor;
                (field as Rectangle).Fill = new SolidColorBrush(Colors.Yellow);
                return;
            }
            // Else if we clicked on the opponent's piece
            else if((playerIndex == 0 && clickedPiece.Color == Core.DTOs.Chess.Color.Black) || (playerIndex == 1 && clickedPiece.Color == Core.DTOs.Chess.Color.White))
            {
                // Do nothing if we haven't selected our piece to move yet
                if(lastTapped == null)
                {
                    return;
                }
            }
            // Perform the move, then reset highlight
            var moveData = new ChessMoveData()
            {
                FromColumn = lastTapped.Column,
                FromRow = lastTapped.Row,
                ToColumn = column,
                ToRow = row,
                MatchId = Match.Id
            };
            MoveCommand.Execute(moveData);
            lastTapped = null;
            lastTappedRect.Fill = new SolidColorBrush(lastTappedRectColor);
            lastTappedRect = null;
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
