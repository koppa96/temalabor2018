namespace Czeum.Core.DTOs.Abstractions.Lobbies
{
    public class LobbySettingsField<TValue>
    {
        public string DisplayName { get; set; }
        public TValue Value { get; set; }
    }
}