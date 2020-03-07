﻿using Czeum.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Czeum.Domain.Entities.Achivements
{
    public class HaveWinRateAchivement : Achivement
    {
        public int Level { get; set; }
        public double WinRate { get; set; }

        public override string Title => $"Több mint szerencse - {Level}. szint";
        public override string Description => $"Legyen legalább {WinRate.ToString("0.##")}%-os nyerési arányod.";

        public override bool CheckCriteria(User user)
        {
            return (double)user.WonMatches.Count / user.Matches.Count(x => x.Match.State == MatchState.Finished) >= WinRate;
        }
    }
}
