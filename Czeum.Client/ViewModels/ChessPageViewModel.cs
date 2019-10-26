using Czeum.Client.Interfaces;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Chess;
using Czeum.Core.DTOs.Extensions;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Services;
using Flurl.Http;
using Prism.Commands;
using Prism.Windows.Mvvm;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Czeum.Client.ViewModels
{
    class ChessPageViewModel : ViewModelBase
    {
        public IMatchStore matchStore { get; }
        private IMatchService matchService;
        private IDialogService dialogService;

        private Tuple<int, int> selectedPiece;
        private Tuple<int, int> selectedField;

        public MatchStatus Match { get => matchStore.SelectedMatch; }
        public string OpponentName { get => Match.Players.Where(x => x.PlayerIndex != Match.PlayerIndex).First().Username; }
        public ICommand FieldSelectedCommand { get; internal set; }
        public ICommand PieceSelectedCommand { get; internal set; }

        public ChessPageViewModel(IMatchStore matchStore, IMatchService matchService, IDialogService dialogService)
        {
            this.matchStore = matchStore;
            this.matchService = matchService;
            this.dialogService = dialogService;

            FieldSelectedCommand = new DelegateCommand<Tuple<int, int>>(async (x) => await FieldSelected(x));
            PieceSelectedCommand = new DelegateCommand<Tuple<int, int>>(PieceSelected);
        }

        private async Task FieldSelected(Tuple<int, int> selectedFieldCoords)
        {
            if (selectedPiece == null)
            {
                return;
            }
            selectedField = selectedFieldCoords;

            var moveData = new ChessMoveData()
            {
                MatchId = matchStore.SelectedMatch.Id,
                FromColumn = selectedPiece.Item1,
                FromRow = selectedPiece.Item2,
                ToColumn = selectedField.Item1,
                ToRow = selectedField.Item2
            };

            MatchStatus result = null;
            try
            {
                result = await matchService.HandleMoveAsync(moveData);
            }
            catch (Flurl.Http.FlurlHttpException e)
            {
                var details = await e.GetResponseJsonAsync<ApiProblemDetails>();
                await dialogService.ShowError(details.Detail);
            }
            if (result != null)
            {
                await matchStore.UpdateMatch(result);
            }

            selectedField = null;
            selectedPiece = null;
        }

        private async void PieceSelected(Tuple<int, int> selectedPieceCoords)
        {
            ChessMoveResult board = (ChessMoveResult)matchStore.SelectedMatch.CurrentBoard.Content;
            var clickedPiece = board.PieceInfos.FirstOrDefault(p => p.Column == selectedPieceCoords.Item1 && p.Row == selectedPieceCoords.Item2);
            //If we clicked on the opponent's piece
            var playerIndex = matchStore.SelectedMatch.CurrentPlayerIndex;
            if ((playerIndex == 0 && clickedPiece.Color == Color.Black) || (playerIndex == 1 && clickedPiece.Color == Color.White))
            {
                await FieldSelected(selectedPieceCoords);
                return;
            }
            selectedPiece = selectedPieceCoords;
        }
    }
}