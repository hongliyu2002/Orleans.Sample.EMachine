namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineUnloadedMoneyEvent : SnackMachineEvent
{
    public SnackMachineUnloadedMoneyEvent(Guid uuId, Guid traceId, string operatedBy)
        : base(uuId, traceId, operatedBy)
    {
    }
}
