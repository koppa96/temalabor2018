using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Client.DTOs
{/*
    public class NotifyingLobbyData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private LobbyData innerLobby;

        public LobbyData InnerLobby {
            get { return innerLobby; }
            set { innerLobby = value; InvokeAllPropertyChange(); }
        }

        private void InvokeAllPropertyChange() {
            OnPropertyChanged("Host");
            OnPropertyChanged("Guest");
            OnPropertyChanged("BoardHeight");
            OnPropertyChanged("BoardWidth");
            OnPropertyChanged("Status");
            OnPropertyChanged("InvitedPlayers");
        }

        public int LobbyId {
            get { return innerLobby.LobbyId; }
            set { innerLobby.LobbyId = value; OnPropertyChanged(); }
        }

        public string Host {
            get { return innerLobby.Host; }
            set { innerLobby.Host = value; OnPropertyChanged(); }
        }

        public string Guest {
            get { return innerLobby.Guest; }
            set { innerLobby.Guest = value; OnPropertyChanged(); }
        }

        public int BoardHeight {
            get { return innerLobby.BoardHeight; }
            set { innerLobby.BoardHeight = value; OnPropertyChanged(); }
        }

        public int BoardWidth {
            get { return innerLobby.BoardWidth; }
            set { innerLobby.BoardWidth = value; OnPropertyChanged(); }
        }

        public LobbyStatus Status {
            get { return innerLobby.Status; }
            set { innerLobby.Status = value; OnPropertyChanged(); }
        }

        public List<string> InvitedPlayers {
            get { return innerLobby.InvitedPlayers; }
            set { innerLobby.InvitedPlayers = value; OnPropertyChanged(); }
        }

        public NotifyingLobbyData(LobbyData data) {
            innerLobby = data;
        }
    }*/
}
