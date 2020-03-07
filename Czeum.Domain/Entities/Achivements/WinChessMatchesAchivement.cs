using Czeum.Domain.Entities.Boards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Domain.Entities.Achivements
{
    public class WinChessMatchesAchivement : WinSpecificMatchesAchivement<SerializedChessBoard>
    {
        public override string GameName => "Sakk";
    }
}
