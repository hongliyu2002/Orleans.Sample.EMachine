using EMachine.Sales.Orleans.States;

namespace EMachine.Sales.Orleans.Mappers;

public static class DomainToStateMapper
{
    public static Money Map(this Domain.Money source)
    {
        return new Money(source.Yuan1, source.Yuan2, source.Yuan5, source.Yuan10, source.Yuan20, source.Yuan50, source.Yuan100);
    }

    public static SnackPile Map(this Domain.SnackPile source)
    {
        return new SnackPile(source.SnackId, source.Quantity, source.Price);
    }

    public static Slot Map(this Domain.Slot source, Guid machineId)
    {
        return new Slot(source.Position, source.SnackPile?.Map());
    }
}
