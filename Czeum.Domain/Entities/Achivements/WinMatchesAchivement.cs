using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Domain.Entities.Achivements
{
    public class WinMatchesAchivement : Achivement
    {
        public int Level { get; set; }
        public int WinCount { get; set; }

        public override string Title => $"Társasjáték bajnok - {Level}. szint";
        public override string Description => $"Nyerj {WinCount} meccset összesen, akármelyik játéktípusból.";

        public override bool CheckCriteria(User user)
        {
            return user.WonMatches.Count >= WinCount;
        }
    }
}
