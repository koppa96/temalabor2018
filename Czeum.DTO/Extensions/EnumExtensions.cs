using System;
using System.Linq;
using System.Reflection;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.DTO.Lobbies;

namespace Czeum.DTO.Extensions
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

        private static GameTypeAttribute GetGameTypeAttribute(this Enum @enum)
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