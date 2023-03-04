using EMachine.Domain.Shared;
using Fluxera.Guards;
using Orleans.Runtime;

namespace EMachine.Domain.Tests;

public interface IMoneyGrain : IGrainWithGuidKey
{
    Task<Money> Get();
    Task Set(Money money);
    Task Plus(Money money);
    Task Minus(Money money);
    Task Multiply(int multiplier);
}

public class MoneyGrain : Grain, IMoneyGrain
{
    private readonly IPersistentState<MoneyState> _money;

    /// <inheritdoc />
    public MoneyGrain([PersistentState("Money", "MemoryStore")] IPersistentState<MoneyState> money)
    {
        _money = money;
    }

    /// <inheritdoc />
    public Task<Money> Get()
    {
        return Task.FromResult(_money.State.Inventory);
    }

    /// <inheritdoc />
    public async Task Set(Money money)
    {
        _money.State = new MoneyState(money);
        await _money.WriteStateAsync();
    }

    /// <inheritdoc />
    public async Task Plus(Money money)
    {
        _money.State.Inventory += money;
        await _money.WriteStateAsync();
    }

    /// <inheritdoc />
    public async Task Minus(Money money)
    {
        _money.State.Inventory -= money;
        await _money.WriteStateAsync();
    }

    /// <inheritdoc />
    public async Task Multiply(int multiplier)
    {
        _money.State.Inventory *= multiplier;
        await _money.WriteStateAsync();
    }
}

[GenerateSerializer]
public sealed class MoneyState
{
    public MoneyState()
    {
        Inventory = Money.Zero;
    }

    /// <inheritdoc />
    public MoneyState(Money inventory)
        : this()
    {
        Inventory = Guard.Against.Null(inventory, nameof(inventory));
    }

    [Id(0)]
    public Money Inventory { get; set; }
}
