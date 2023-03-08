using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackWriterDeleteOneCommand : SnackWriterCommand
{
    public SnackWriterDeleteOneCommand(Guid uuId, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        UuId = Guard.Against.Empty(uuId, nameof(uuId));
    }

    [Id(0)]
    public Guid UuId { get; }
}
