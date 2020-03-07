using Czeum.Domain.Entities.Boards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Domain.Entities.Achivements
{
    public class WinConnect4MatchesAchivement : WinSpecificMatchesAchivement<SerializedConnect4Board>
    {
        public override string GameName => "Connect4";
    }
}
