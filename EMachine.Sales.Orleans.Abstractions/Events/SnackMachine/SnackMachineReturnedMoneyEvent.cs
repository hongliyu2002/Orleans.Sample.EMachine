namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineReturnedMoneyEvent : SnackMachineEvent
{
    public SnackMachineReturnedMoneyEvent(Guid key, Guid traceId, string operatedBy)
        : base(key, traceId, operatedBy)
    {
    }
}
