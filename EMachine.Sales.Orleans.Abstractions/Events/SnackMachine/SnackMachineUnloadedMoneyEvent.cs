namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineUnloadedMoneyEvent : SnackMachineEvent
{
    public SnackMachineUnloadedMoneyEvent(Guid key, Guid traceId, string operatedBy)
        : base(key, traceId, operatedBy)
    {
    }
}
