using Czeum.Client.Interfaces;
using Czeum.Core.DTOs;
using Czeum.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Czeum.Client.Models
{
    class MessageStore : IMessageStore
    {
        private IMessageService messageService;

        public ObservableCollection<Message> Messages { get; private set; } = new ObservableCollection<Message>();


        public MessageStore(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        public async Task SetMessages(IEnumerable<Message> messages)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Messages.Clear();
                foreach (var message in messages)
                {
                    Messages.Add(message);
                }
            });
        }

        public async Task AddMessage(Message message)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Messages.Add(message);
            });
        }
    }
}
