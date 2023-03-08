using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackNameChangeCommand : SnackCommand
{
    public SnackNameChangeCommand(string name, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
    }

    [Id(0)]
    public string Name { get; }
}
