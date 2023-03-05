using Fluxera.Enumeration;

namespace EMachine.Sales.Domain.Rules;

public sealed class SnackMachineErrorCodes : Enumeration<SnackMachineErrorCodes, int>
{
    public static readonly SnackMachineErrorCodes SnackMachineIsNotInitialized = new(1, nameof(SnackMachineIsNotInitialized));
    public static readonly SnackMachineErrorCodes SnackMachineAlreadyExists = new(2, nameof(SnackMachineAlreadyExists));
    public static readonly SnackMachineErrorCodes SnackMachineHasRemoved = new(3, nameof(SnackMachineHasRemoved));
    public static readonly SnackMachineErrorCodes SnackMachineInTransaction = new(4, nameof(SnackMachineInTransaction));
    public static readonly SnackMachineErrorCodes SnackMachineSingleCoinOrNoteRequired = new(5, nameof(SnackMachineSingleCoinOrNoteRequired));
    
    /// <inheritdoc />
    public SnackMachineErrorCodes(int value, string name)
        : base(value, name)
    {
    }
}
