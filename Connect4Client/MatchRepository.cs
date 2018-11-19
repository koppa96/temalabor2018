using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Connect4Dtos;
using Windows.UI.Core;

namespace Connect4Client {
    class MatchRepository {
        public static MatchRepository Instance { get; } = new MatchRepository();
        private static object lockObject = new object();

        private ObservableCollection<MatchDto> matchList;
        public ObservableCollection<MatchDto> MatchList { get { return matchList; } }

        public MatchDto SelectedMatch { get; set; }

        private MatchRepository(){}

        public void LoadItems(ICollection<MatchDto> matches) {
            matchList = new ObservableCollection<MatchDto>(matches);
        }

#pragma warning disable CS4014
        public void AddItem(MatchDto match) {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                matchList.Add(match);
            });
        }
    }
#pragma warning restore CS4014
}
