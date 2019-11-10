using Czeum.Client.Interfaces;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.Services;
using Flurl.Http;
using Prism.Commands;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private IMessageService messageService;
        private IMessageStore messageStore;

        public ICommand PerformMoveCommand { get; internal set; }
        public ICommand SendMessageCommand { get; private set; }

        public MatchStatus Match => matchStore.SelectedMatch; 
        public string Username => userManagerService.Username;

        private string messageText;
        public string MessageText {
            get => messageText;
            set => SetProperty(ref messageText, value);
        }

        public ObservableCollection<Message> Messages => messageStore.Messages;

        public MatchDetailsPageViewModel(
            IMatchStore matchStore, 
            IMatchService matchService, 
            IDialogService dialogService,
            IUserManagerService userManagerService,
            IMessageService messageService,
            IMessageStore messageStore
            )
        {
            this.matchStore = matchStore;
            this.matchService = matchService;
            this.dialogService = dialogService;
            this.userManagerService = userManagerService;
            this.messageService = messageService;
            this.messageStore = messageStore;

            PerformMoveCommand = new DelegateCommand<MoveData>(PerformMove);
            SendMessageCommand = new DelegateCommand(SendMessage);
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

        private async void SendMessage()
        {
            if (string.IsNullOrEmpty(MessageText))
            {
                return;
            }
            try
            {
                var messageResult = await messageService.SendToMatchAsync(matchStore.SelectedMatch.Id, MessageText);
                await messageStore.AddMessage(messageResult);
                MessageText = "";
            }
            catch (FlurlHttpException e)
            {
                await dialogService.ShowError("Could not send message");
            }
        }
    }
}
