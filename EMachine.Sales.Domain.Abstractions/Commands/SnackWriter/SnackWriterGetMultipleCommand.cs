using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackWriterGetMultipleCommand : SnackWriterCommand
{
    public SnackWriterGetMultipleCommand(long[] ids, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Ids = Guard.Against.Null(ids, nameof(ids));
    }

    [Id(0)]
    public long[] Ids { get; }
}
