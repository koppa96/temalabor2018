using Czeum.Domain.Entities.Achivements;
using Czeum.Domain.Entities.Boards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.DAL.Seed
{
    public static class AchivementSeed
    {
        public static List<DoMovesAchivement> DoMovesAchivements = new List<DoMovesAchivement>
        {
            new DoMovesAchivement
            {
                Id = Guid.Parse("11b89499-17be-4c93-b561-b6cf176f1a52"),
                Level = 1,
                MoveCount = 1
            },
            new DoMovesAchivement
            {
                Id = Guid.Parse("b23e061b-fd81-4335-a843-e34bc78679b1"),
                Level = 2,
                MoveCount = 500
            },
            new DoMovesAchivement
            {
                Id = Guid.Parse("5f86c4fe-8d0b-40bf-be7f-e5f374184d9c"),
                Level = 3,
                MoveCount = 5000
            }
        };

        public static List<HaveWinRateAchivement> HaveWinRateAchivements = new List<HaveWinRateAchivement>
        {
            new HaveWinRateAchivement
            {
                Id = Guid.Parse("68e19be4-e065-4231-9806-bf40a9d0b004"),
                Level = 1,
                WinRate = 0.6
            },
            new HaveWinRateAchivement
            {
                Id = Guid.Parse("855904dd-16ae-41ee-a5c2-cdfc6f4a914f"),
                Level = 2,
                WinRate = 0.7
            },
            new HaveWinRateAchivement
            {
                Id = Guid.Parse("d62bf078-d3e3-40c5-92cb-a6dda1246327"),
                Level = 3,
                WinRate = 0.8
            }
        };

        public static List<WinMatchesAchivement> WinMatchesAchivements = new List<WinMatchesAchivement>
        {
            new WinMatchesAchivement
            {
                Id = Guid.Parse("69d3d5b7-de45-4bda-b20f-0f4cd5ac3340"),
                Level = 1,
                WinCount = 10
            },
            new WinMatchesAchivement
            {
                Id = Guid.Parse("d742e2b6-8dd3-4fb8-b80a-728d686251b4"),
                Level = 2,
                WinCount = 100
            },
            new WinMatchesAchivement
            {
                Id = Guid.Parse("5bcac04d-2bc4-40ad-af06-4e8cff05945f"),
                Level = 3,
                WinCount = 1000
            }
        };

        public static List<WinQuickMatchesAchivement> WinQuickMatchesAchivements = new List<WinQuickMatchesAchivement>
        {
            new WinQuickMatchesAchivement
            {
                Id = Guid.Parse("afa121e1-6dd5-49ea-88d2-5cfd538256f5"),
                Level = 1,
                WinCount = 5
            },
            new WinQuickMatchesAchivement
            {
                Id = Guid.Parse("7ce1cbff-3fca-4728-9be1-6f632a5e9358"),
                Level = 2,
                WinCount = 50
            },
            new WinQuickMatchesAchivement
            {
                Id = Guid.Parse("12944108-9bb0-43e3-b1c8-f3eac1331442"),
                Level = 3,
                WinCount = 500
            }
        };

        public static List<WinConnect4MatchesAchivement> WinConnect4MatchesAchivements = new List<WinConnect4MatchesAchivement>
        {
            new WinConnect4MatchesAchivement
            {
                Id = Guid.Parse("9982fdd1-8026-4e55-a448-62218cfa4b0a"),
                Level = 1,
                WinCount = 1
            },
            new WinConnect4MatchesAchivement
            {
                Id = Guid.Parse("22c4ee6b-7451-4c81-8010-0ab28571daf7"),
                Level = 2,
                WinCount = 25
            },
            new WinConnect4MatchesAchivement
            {
                Id = Guid.Parse("24d59ba8-ff4e-442c-b76f-fbf524e5514d"),
                Level = 3,
                WinCount = 100
            }
        };

        public static List<WinChessMatchesAchivement> WinChessMatchesAchivements => new List<WinChessMatchesAchivement>
        {
            new WinChessMatchesAchivement
            {
                Id = Guid.Parse("205a0c88-45e4-4bf4-bb0a-be66c18dcd86"),
                Level = 1,
                WinCount = 1
            },
            new WinChessMatchesAchivement
            {
                Id = Guid.Parse("dc3fd2c0-c173-4012-b51d-e64fdd7680eb"),
                Level = 2,
                WinCount = 25
            },
            new WinChessMatchesAchivement
            {
                Id = Guid.Parse("e18f8521-0978-4cf4-b123-e88bccaf992e"),
                Level = 3,
                WinCount = 100
            }
        };
    }
}
