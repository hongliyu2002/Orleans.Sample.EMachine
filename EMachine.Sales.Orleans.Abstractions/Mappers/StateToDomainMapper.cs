using EMachine.Sales.Domain;

namespace EMachine.Sales.Orleans.Mappers;

public static class StateToDomainMapper
{
    public static Money Map(this States.Money source)
    {
        return new Money
               {
                   Yuan1 = source.Yuan1,
                   Yuan2 = source.Yuan2,
                   Yuan5 = source.Yuan5,
                   Yuan10 = source.Yuan10,
                   Yuan20 = source.Yuan20,
                   Yuan50 = source.Yuan50,
                   Yuan100 = source.Yuan100,
                   Amount = source.Amount
               };
    }

    public static SnackPile Map(this States.SnackPile source)
    {
        return new SnackPile
               {
                   SnackId = source.SnackId,
                   Quantity = source.Quantity,
                   Price = source.Price,
                   TotalPrice = source.TotalPrice
               };
    }

    public static Slot Map(this States.Slot source, Guid machineId)
    {
        return new Slot
               {
                   MachineId = machineId,
                   Position = source.Position,
                   SnackPile = source.SnackPile?.Map()
               };
    }
}
