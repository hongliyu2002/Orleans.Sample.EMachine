using Fluxera.Enumeration;

namespace EMachine.Sales.Domain.Rules;

public sealed class SnackErrorCodes : Enumeration<SnackErrorCodes, int>
{
    public static readonly SnackErrorCodes SnackIsNotInitialized = new(1, nameof(SnackIsNotInitialized));
    public static readonly SnackErrorCodes SnackAlreadyExists = new(2, nameof(SnackAlreadyExists));
    public static readonly SnackErrorCodes SnackHasRemoved = new(3, nameof(SnackHasRemoved));

    /// <inheritdoc />
    public SnackErrorCodes(int value, string name)
        : base(value, name)
    {
    }
}
