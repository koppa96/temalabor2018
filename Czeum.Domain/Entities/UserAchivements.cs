using Czeum.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Domain.Entities
{
    public class UserAchivements : EntityBase
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid AchivementId { get; set; }
        public Achivement Achivement { get; set; }
        public DateTime UnlockedAt { get; set; }
    }
}
