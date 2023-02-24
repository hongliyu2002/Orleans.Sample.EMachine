using System;
using JetBrains.Annotations;

namespace EMachine.Domain.Shared;

[PublicAPI]
public interface ITraceable
{
    Guid TraceId { get; }

    public string OperatedBy { get; }
}
