using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Czeum.DTO;
using Windows.UI.Core;

namespace Czeum.Client {/*
    class MatchRepository {
        public static MatchRepository Instance { get; } = new MatchRepository();
        private static object lockObject = new object();

        private ObservableCollection<MatchDto> matchList;
        private MatchesPage matchesPage;

        public ObservableCollection<MatchDto> MatchList { get { return matchList; } }

        public MatchDto SelectedMatch { get; set; }

        private MatchRepository(){}

        public void LoadItems(ICollection<MatchDto> matches) {
            matchList = new ObservableCollection<MatchDto>(matches.OrderBy(x => x.State));
        }

#pragma warning disable CS4014
        public void AddItem(MatchDto match) {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                matchList.Add(match);
            });
        }

        public void RefreshMatch(MatchDto match) {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                int MatchId = SelectedMatch?.MatchId ?? -1;

                MatchDto matchInList = MatchList.SingleOrDefault(x => x.MatchId == match.MatchId);
                int indexInList = MatchList.IndexOf(matchInList);
                MatchList[indexInList] = match;

                if(match.MatchId == MatchId) {
                    SelectedMatch = match;
                }
                matchesPage.DrawBoard();
            });
        }

        internal void AddMatchPage(MatchesPage matchesPage) {
            this.matchesPage = matchesPage;
        }
    }
#pragma warning restore CS4014*/
}
