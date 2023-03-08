using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineWriterGetMultipleCommand : SnackMachineWriterCommand
{
    public SnackMachineWriterGetMultipleCommand(Guid[] ids, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Ids =  Guard.Against.Null(ids, nameof(ids));
    }

    [Id(0)]
    public Guid[] Ids { get; }
}
