using System;
using Orleans;

namespace EMachine.Sales.Domain.Abstractions.Commands;

[Immutable]
[Serializable]
[GenerateSerializer]
public sealed class SnackRemoveCommand : SnackCommand
{
    public SnackRemoveCommand()
    {
    }

    public SnackRemoveCommand(Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
    }
}
