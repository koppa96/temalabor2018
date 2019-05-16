using Czeum.Client.Interfaces;
using Czeum.DTO;
using Czeum.DTO.Chess;
using Prism.Commands;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Czeum.Client.ViewModels
{
    class ChessPageViewModel : ViewModelBase
    {
        public IMatchStore matchStore { get; }
        private IMatchService matchService;

        private Tuple<int, int> selectedPiece;
        private Tuple<int, int> selectedField;

        public MatchStatus Match { get => matchStore.SelectedMatch; }
        public ICommand FieldSelectedCommand { get; internal set; }
        public ICommand PieceSelectedCommand { get; internal set; }

        public ChessPageViewModel(IMatchStore matchStore, IMatchService matchService)
        {
            this.matchStore = matchStore;
            this.matchService = matchService;

            FieldSelectedCommand = new DelegateCommand<Tuple<int,int>>(FieldSelected);
            PieceSelectedCommand = new DelegateCommand<Tuple<int,int>>(PieceSelected);
        }

        private void FieldSelected(Tuple<int, int> selectedFieldCoords)
        {
            if(selectedPiece == null)
            {
                return;
            }
            selectedField = selectedFieldCoords;

            var moveData = new ChessMoveData() { MatchId = matchService.CurrentMatch.MatchId, FromColumn = selectedPiece.Item1, FromRow = selectedPiece.Item2, ToColumn = selectedField.Item1, ToRow = selectedField.Item2};
            matchService.DoMove(moveData);

            selectedField = null;
            selectedPiece = null;
        }

        private void PieceSelected(Tuple<int, int> selectedPieceCoords)
        {
            ChessMoveResult moveResult = (ChessMoveResult) matchService.CurrentMatch.CurrentBoard;
            var clickedPiece = moveResult.PieceInfos.FirstOrDefault(p => p.Column == selectedPieceCoords.Item1 && p.Row == selectedPieceCoords.Item2);
            //If we clicked on the opponent's piece
            if((matchService.CurrentMatch.PlayerId == 1 && clickedPiece.Color == Color.Black)
                || (matchService.CurrentMatch.PlayerId == 2 && clickedPiece.Color == Color.White))
            {
                FieldSelected(selectedPieceCoords);
                return;
            }
            selectedPiece = selectedPieceCoords;
        }
    }
}
