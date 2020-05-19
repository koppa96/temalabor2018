using System;
using Czeum.Core.Domain;

namespace Czeum.Domain.Entities
{
    public class UserMatch : EntityBase
    {
        public User User { get; set; }
        public Guid? UserId { get; set; }
        public Match Match { get; set; }
        public Guid? MatchId { get; set; }
        public int PlayerIndex { get; set; }
        public bool VotesForDraw { get; set; }
        public bool Resigned { get; set; }
    }
}