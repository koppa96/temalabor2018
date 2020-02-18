using System;
using System.Collections.Generic;
using Czeum.Core.Domain;

namespace Czeum.Domain.Entities
{
    public class Friendship : EntityBase
    {
        public User User1 { get; set; }
        public Guid? User1Id { get; set; }
        public User User2 { get; set; }
        public Guid? User2Id { get; set; }

        public List<DirectMessage> Messages { get; set; }
    }
}
