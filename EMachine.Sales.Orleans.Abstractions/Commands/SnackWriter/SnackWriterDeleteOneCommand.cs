using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackWriterDeleteOneCommand : SnackWriterCommand
{
    public SnackWriterDeleteOneCommand(Guid key, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Key = Guard.Against.Empty(key, nameof(key));
    }

    [Id(0)]
    public Guid Key { get; }
}
