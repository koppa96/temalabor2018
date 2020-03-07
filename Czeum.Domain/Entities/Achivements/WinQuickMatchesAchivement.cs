using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Czeum.Domain.Entities.Achivements
{
    public class WinQuickMatchesAchivement : Achivement
    {
        public int Level { get; set; }
        public int WinCount { get; set; }

        public override string Title => $"Gyorsjáték bajnok - {Level}. szint";
        public override string Description => $"Nyerj {WinCount} gyorsjátékot.";

        public override bool CheckCriteria(User user)
        {
            return user.WonMatches.Count(x => x.IsQuickMatch) >= WinCount;
        }
    }
}
