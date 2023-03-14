using JetBrains.Annotations;

namespace EMachine.Sales.Domain;

[PublicAPI]
public sealed class SnackId
{
    public Guid Id { get; set; }
}
