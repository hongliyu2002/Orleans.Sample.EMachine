using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackWriterGetMultipleCommand : SnackWriterCommand
{
    public SnackWriterGetMultipleCommand(Guid[] uuIds, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        UuIds =  Guard.Against.Null(uuIds, nameof(uuIds));
    }

    [Id(0)]
    public Guid[] UuIds { get; }
}
