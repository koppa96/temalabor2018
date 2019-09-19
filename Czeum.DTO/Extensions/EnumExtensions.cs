using System;
using System.Linq;
using System.Reflection;
using Czeum.Abstractions.DTO.Lobbies;

namespace Czeum.DTO.Extensions
{
    public static class EnumExtensions
    {
        public static Type GetLobbyType(this Enum lobbyType)
        {
            var enumType = lobbyType.GetType();
            var memberInfo = enumType.GetMember(lobbyType.ToString());

            if (memberInfo.Length > 0)
            {
                var attribute = memberInfo.First().GetCustomAttribute<LobbyTypeAttribute>();
                if (attribute == null)
                {
                    throw new InvalidOperationException("There is no LobbyType attribute.");
                }

                return attribute.LobbyType;
            }
            else
            {
                throw new InvalidOperationException("Unknown enum value.");
            }
        }
    }
}