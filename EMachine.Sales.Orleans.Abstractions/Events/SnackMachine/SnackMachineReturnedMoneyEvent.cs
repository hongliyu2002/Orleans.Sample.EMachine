namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineReturnedMoneyEvent : SnackMachineEvent
{
    public SnackMachineReturnedMoneyEvent(Guid uuId, Guid traceId, string operatedBy)
        : base(uuId, traceId, operatedBy)
    {
    }
}
