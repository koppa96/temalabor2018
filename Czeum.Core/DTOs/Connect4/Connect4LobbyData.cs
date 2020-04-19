using Czeum.Core.DTOs.Abstractions.Lobbies;

namespace Czeum.Core.DTOs.Connect4
{
    /// <summary>
    /// A lobby that is used to create Connect4 games.
    /// </summary>
    public class Connect4LobbyData : LobbyDataWithSettings<Connect4LobbySettings>
    {
        public Connect4LobbyData()
        {
            Settings = new Connect4LobbySettings()
            {
                BoardWidth = new LobbySettingsField<int>()
                {
                    DisplayName = "Szélesség",
                    Value = 7
                },
                BoardHeight = new LobbySettingsField<int>()
                {
                    DisplayName = "Magasság",
                    Value = 6
                }
            };
        }

        public override int MinimumPlayerCount => 2;
        public override int MaximumPlayerCount => 2;

        public override bool ValidateSettings()
        {
            // The height should be between 4 - 10, the width between 4-15
            return Settings.BoardWidth.Value > 3 && Settings.BoardWidth.Value < 16 &&
                   Settings.BoardHeight.Value > 3 && Settings.BoardWidth.Value < 11;
        }
    }
}