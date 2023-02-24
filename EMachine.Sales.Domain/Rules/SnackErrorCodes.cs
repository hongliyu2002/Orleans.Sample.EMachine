using Fluxera.Enumeration;

namespace EMachine.Sales.Domain.Rules;

public sealed class SnackErrorCodes : Enumeration<SnackErrorCodes, int>
{
    public static readonly SnackErrorCodes SnackDoesNotExist = new(1, nameof(SnackDoesNotExist));
    public static readonly SnackErrorCodes SnackAlreadyExist = new(1, nameof(SnackAlreadyExist));
    public static readonly SnackErrorCodes SnackHasRemoved = new(1, nameof(SnackHasRemoved));

    /// <inheritdoc />
    public SnackErrorCodes(int value, string name)
        : base(value, name)
    {
    }
}
