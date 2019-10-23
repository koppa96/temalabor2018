using Czeum.Core.DTOs.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Client.Extensions
{
    static class MoveResultWrapperExtensions
    {
        public static PageTokens GetPageToken(this MoveResultWrapper wrapper)
        {
            switch (wrapper.GameType)
            {
                case Core.Enums.GameType.Chess:
                    return PageTokens.Chess;
                case Core.Enums.GameType.Connect4:
                    return PageTokens.Connect4;
            }
            return PageTokens.Match;
        }
    }
}
