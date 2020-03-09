using System;
using System.Linq;
using System.Reflection;
using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.DTOs.Abstractions.Lobbies;
using Czeum.Core.Enums;
using Czeum.Core.GameServices;

namespace Czeum.Core.DTOs.Extensions
{
    public static class EnumExtensions
    {
        public static Type GetLobbyType(this Enum @enum)
        {
            return @enum.GetGameTypeAttribute().LobbyType;
        }

        public static Type GetMoveDataType(this Enum @enum)
        {
            return @enum.GetGameTypeAttribute().MoveDataType;
        }

        public static Type GetMoveResultType(this Enum @enum)
        {
            return @enum.GetGameTypeAttribute().MoveResultType;
        }

        public static GameType GetGameType(this LobbyData lobbyData)
        {
            return Enum.GetValues(typeof(GameType)).Cast<GameType>()
                .First(v => v.GetGameTypeAttribute().LobbyType == lobbyData.GetType());
        }

        public static GameType GetGameType(this IMoveResult moveResult)
        {
            return Enum.GetValues(typeof(GameType)).Cast<GameType>()
                .First(v => v.GetGameTypeAttribute().MoveResultType == moveResult.GetType());
        }

        public static GameType GetGameType(this MoveData moveData)
        {
            return Enum.GetValues(typeof(GameType)).Cast<GameType>()
                .First(v => v.GetGameTypeAttribute().MoveDataType == moveData.GetType());
        }

        public static GameTypeAttribute GetGameTypeAttribute(this Enum @enum)
        {
            var enumType = @enum.GetType();
            var memberInfo = enumType.GetMember(@enum.ToString());

            if (memberInfo.Length > 0)
            {
                return memberInfo.First().GetCustomAttribute<GameTypeAttribute>() ??
                    throw new InvalidOperationException("There is no GameType attribute.");
            }

            throw new InvalidOperationException("Unknown enum value");
        }
    }
}