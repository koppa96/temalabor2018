using Czeum.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Czeum.Domain.Entities.Achivements
{
    public abstract class WinSpecificMatchesAchivement<TSerializedBoard> : Achivement
        where TSerializedBoard : SerializedBoard
    {
        public int Level { get; set; }
        public int WinCount { get; set; }
        public abstract string GameName { get; }

        public override string Title => $"{GameName} király - {Level}. szint";
        public override string Description => $"Nyerj {WinCount} {GameName} meccset.";

        public override bool CheckCriteria(User user)
        {
            return user.WonMatches.Count(x => x.Board is TSerializedBoard) >= WinCount;
        }
    }
}
