using Czeum.Abstractions;

namespace Czeum.DAL.Entities
{
    public abstract class SerializedBoard : ISerializedBoard
    {
        public int BoardId { get; set; }
        public string BoardData { get; set; }

        public Match Match { get; set; }
        public int MatchId { get; set; }
    }
}
