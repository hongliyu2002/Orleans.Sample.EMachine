using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineWriterGetMultipleCommand : SnackMachineWriterCommand
{
    public SnackMachineWriterGetMultipleCommand(Guid[] keys, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Keys =  Guard.Against.Null(keys, nameof(keys));
    }

    [Id(0)]
    public Guid[] Keys { get; }
}
