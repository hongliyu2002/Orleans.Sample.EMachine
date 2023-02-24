using System;
using Fluxera.Guards;
using Orleans;

namespace EMachine.Sales.Domain.Abstractions.Commands;

[Immutable]
[Serializable]
[GenerateSerializer]
public sealed class SnackNameChangeCommand : SnackCommand
{
    public SnackNameChangeCommand()
    {
        Name = string.Empty;
    }

    public SnackNameChangeCommand(string name, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
    }

    [Id(0)]
    public string Name { get; set; }
}
