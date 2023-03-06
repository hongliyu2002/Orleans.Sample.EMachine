using Fluxera.Enumeration;

namespace EMachine.Sales.Domain.Rules;

public sealed class ErrorCodes : Enumeration<ErrorCodes, int>
{
    public static readonly ErrorCodes SnackNotInitialized = new(101, nameof(SnackNotInitialized));
    public static readonly ErrorCodes SnackExists = new(102, nameof(SnackExists));
    public static readonly ErrorCodes SnackRemoved = new(103, nameof(SnackRemoved));

    public static readonly ErrorCodes SnackMachineNotInitialized = new(201, nameof(SnackMachineNotInitialized));
    public static readonly ErrorCodes SnackMachineExists = new(202, nameof(SnackMachineExists));
    public static readonly ErrorCodes SnackMachineRemoved = new(203, nameof(SnackMachineRemoved));
    public static readonly ErrorCodes SnackMachineInTransaction = new(204, nameof(SnackMachineInTransaction));
    public static readonly ErrorCodes SnackMachineNotInTransaction = new(205, nameof(SnackMachineNotInTransaction));
    public static readonly ErrorCodes SnackMachineSingleCoinOrNoteRequired = new(206, nameof(SnackMachineSingleCoinOrNoteRequired));
    public static readonly ErrorCodes SnackMachineSlotNotExists = new(207, nameof(SnackMachineSlotNotExists));
    public static readonly ErrorCodes SnackMachineSlotSnackPileNotExists = new(208, nameof(SnackMachineSlotSnackPileNotExists));
    public static readonly ErrorCodes SnackMachineSlotSnackPileNotEnoughSnack = new(209, nameof(SnackMachineSlotSnackPileNotEnoughSnack));
    public static readonly ErrorCodes SnackMachineNotEnoughMoney = new(210, nameof(SnackMachineNotEnoughMoney));
    public static readonly ErrorCodes SnackMachineNotEnoughChange = new(211, nameof(SnackMachineNotEnoughChange));

    /// <inheritdoc />
    public ErrorCodes(int value, string name)
        : base(value, name)
    {
    }
}
