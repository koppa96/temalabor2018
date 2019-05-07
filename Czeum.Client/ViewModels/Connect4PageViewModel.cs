using Czeum.Client.Interfaces;
using Czeum.DTO;
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
    class Connect4PageViewModel : ViewModelBase
    {
        private IMatchStore matchStore;

        public MatchStatus Match { get => matchStore.SelectedMatch; }

        public ICommand ObjectPlacedCommand { get; set; }

        public Connect4PageViewModel(IMatchStore matchStore)
        {
            this.matchStore = matchStore;
            ObjectPlacedCommand = new DelegateCommand<Tuple<int, int>>(ObjectPlaced);
        }

        private void ObjectPlaced(Tuple<int, int> position)
        {
            ;
        }

    }
}
