using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackWriterDeleteOneCommand : SnackWriterCommand
{
    public SnackWriterDeleteOneCommand(Guid id, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Id = Guard.Against.Empty(id, nameof(id));
    }

    [Id(0)]
    public Guid Id { get; }
}
