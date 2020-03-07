using Czeum.Core.Domain;
using Czeum.Domain.Entities.Achivements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Domain.Entities
{
    public class UserAchivement : EntityBase
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid AchivementId { get; set; }
        public Achivement Achivement { get; set; }
        public DateTime UnlockedAt { get; set; }
        public bool IsStarred { get; set; }
    }
}
