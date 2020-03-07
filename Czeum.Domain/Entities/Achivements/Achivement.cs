using Czeum.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Czeum.Domain.Entities.Achivements
{
    public abstract class Achivement : EntityBase
    {
        [NotMapped]
        public abstract string Title { get; }

        [NotMapped]
        public abstract string Description { get; }

        public abstract bool CheckCriteria(User user);
    }
}
