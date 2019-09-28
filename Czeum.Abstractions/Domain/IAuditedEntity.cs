using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Abstractions.Domain
{
    public interface IAuditedEntity
    {
        DateTime Created { get; }
        DateTime LastModified { get; }
    }
}
