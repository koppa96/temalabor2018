using System;
using Czeum.Core.Domain;

namespace Czeum.Domain.Entities
{
    public class Friendship : EntityBase
    {
        public User User1 { get; set; }
        public Guid? User1Id { get; set; }
        public User User2 { get; set; }
        public Guid? User2Id { get; set; }
    }
}
