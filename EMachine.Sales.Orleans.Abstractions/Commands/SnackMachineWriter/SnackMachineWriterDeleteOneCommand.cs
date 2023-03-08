using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineWriterDeleteOneCommand : SnackMachineWriterCommand
{
    public SnackMachineWriterDeleteOneCommand(Guid key, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Key = Guard.Against.Empty(key, nameof(key));
    }

    [Id(0)]
    public Guid Key { get; }
}
