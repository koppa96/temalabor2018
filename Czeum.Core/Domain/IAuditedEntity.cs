using System;

namespace Czeum.Core.Domain
{
    public interface IAuditedEntity
    {
        DateTime Created { get; }
        DateTime LastModified { get; }
    }
}
