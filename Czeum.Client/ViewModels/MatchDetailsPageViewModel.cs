using Czeum.Client.Interfaces;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.Services;
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
    public class MatchDetailsPageViewModel : ViewModelBase
    {
        public IMatchStore matchStore { get; }
        private IMatchService matchService;
        private IDialogService dialogService;
        private IUserManagerService userManagerService;

        public ICommand PerformMoveCommand { get; internal set; }

        public MatchStatus Match => matchStore.SelectedMatch; 
        public string Username => userManagerService.Username;

        public MatchDetailsPageViewModel(
            IMatchStore matchStore, 
            IMatchService matchService, 
            IDialogService dialogService,
            IUserManagerService userManagerService
            )
        {
            this.matchStore = matchStore;
            this.matchService = matchService;
            this.dialogService = dialogService;
            this.userManagerService = userManagerService;

            PerformMoveCommand = new DelegateCommand<MoveData>(PerformMove);
        }

        private async void PerformMove(MoveData moveData)
        {
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
        }
    }
}
