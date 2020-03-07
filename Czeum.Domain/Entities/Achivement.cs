using Czeum.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Domain.Entities
{
    public abstract class Achivement : EntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public abstract bool CheckCriterias(User user);
    }
}
