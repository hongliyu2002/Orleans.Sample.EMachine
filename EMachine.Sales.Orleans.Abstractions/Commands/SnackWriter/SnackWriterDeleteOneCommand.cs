using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackWriterDeleteOneCommand : SnackWriterCommand
{
    public SnackWriterDeleteOneCommand(long id, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Id = Guard.Against.Negative(id, nameof(id));
    }

    [Id(0)]
    public long Id { get; }
}
