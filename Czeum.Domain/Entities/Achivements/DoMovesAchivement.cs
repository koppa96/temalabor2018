using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Domain.Entities.Achivements
{
    public class DoMovesAchivement : Achivement
    {
        public int Level { get; set; }
        public int MoveCount { get; set; }

        public override string Title => $"Kis lépés az emberiségnek... - {Level} szint";
        public override string Description => $"Tedd meg a(z) {MoveCount}. szabályos lépésed akármelyik játékban.";

        public override bool CheckCriteria(User user)
        {
            return user.MoveCount >= MoveCount;
        }
    }
}
