using JetBrains.Annotations;

namespace EMachine.Sales.Domain;

[PublicAPI]
public sealed class SnackMachineId
{
    public Guid Id { get; set; }
}
