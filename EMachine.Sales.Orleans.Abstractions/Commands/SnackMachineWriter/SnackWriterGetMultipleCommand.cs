using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineWriterGetMultipleCommand : SnackMachineWriterCommand
{
    public SnackMachineWriterGetMultipleCommand(Guid[] uuIds, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        UuIds =  Guard.Against.Null(uuIds, nameof(uuIds));
    }

    [Id(0)]
    public Guid[] UuIds { get; }
}
