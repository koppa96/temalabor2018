using Czeum.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Client.Interfaces
{
    public interface IMatchStore
    {
        Task AddMatch(MatchStatus match);
        Task RemoveMatch(Guid matchId);
        Task UpdateMatch(MatchStatus match);
        Task ClearMatches();

        ObservableCollection<MatchStatus> MatchList { get; }
        MatchStatus SelectedMatch{ get; set; }

        void SelectMatch(MatchStatus match);
        Task AddMatches(IEnumerable<MatchStatus> matches);
    }
}
