using Czeum.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Core.DTOs.Statistics
{
    public class StatisticsDto
    {
        public int PlayedGames { get; set; }
        public int WonGames { get; set; }
        public GameType FavouriteGame { get; set; }
        public int PlayedGamesOfFavourite { get; set; }
        public int WonGamesOfFavourite { get; set; }
        public string MostPlayedWithName { get; set; }
    }
}
