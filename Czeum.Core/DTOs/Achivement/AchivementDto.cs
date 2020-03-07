using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Core.DTOs.Achivement
{
    public class AchivementDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsStarred { get; set; }
        public DateTime UnlockedAt { get; set; }
    }
}
