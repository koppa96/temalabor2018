using Czeum.Abstractions.DTO;
using Czeum.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Client.Interfaces
{
    public interface IMatchService
    {
        ObservableCollection<MatchStatus> MatchList { get; }
        MatchStatus CurrentMatch{ get; }

        Task QueryMatchList();
        Task DoMove(int column);
        void OpenMatch(MatchStatus match);
    }
}
