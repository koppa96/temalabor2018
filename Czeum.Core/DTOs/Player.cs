namespace Czeum.Core.DTOs
{
    public class Player
    {
        public string Username { get; set; }
        public int PlayerIndex { get; set; }
        public bool VotesForDraw { get; set; }
        public bool Resigned { get; set; }
    }
}