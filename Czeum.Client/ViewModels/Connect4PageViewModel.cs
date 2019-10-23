using Czeum.Client.Interfaces;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Connect4;
using Czeum.Core.Services;
using Prism.Commands;
using Prism.Windows.Mvvm;
using System;
using System.Windows.Input;

namespace Czeum.Client.ViewModels
{
    class Connect4PageViewModel : ViewModelBase
    {
        public IMatchStore matchStore { get; }
        private IMatchService matchService;

        public MatchStatus Match { get => matchStore.SelectedMatch; }

        public ICommand ObjectPlacedCommand { get; set; }

        public Connect4PageViewModel(IMatchStore matchStore, IMatchService matchService)
        {
            this.matchStore = matchStore;
            this.matchService = matchService;

            ObjectPlacedCommand = new DelegateCommand<Tuple<int, int>>(ObjectPlaced);
        }

        private async void ObjectPlaced(Tuple<int, int> position)
        {
            var moveData = new Connect4MoveData() { 
                MatchId = matchStore.SelectedMatch.Id,
                Column = position.Item2 
            };
            var result = await matchService.HandleMoveAsync(moveData);
            if (result != null)
            {
                await matchStore.UpdateMatch(result);
            }
        }

    }
}
