using EMachine.Orleans.Shared;
using EMachine.Orleans.Shared.Events;
using EMachine.Sales.Orleans.States;
using Fluxera.Guards;
using Microsoft.Extensions.Logging;
using Orleans.Concurrency;
using Orleans.FluentResults;
using Orleans.Providers;

namespace EMachine.Sales.Orleans.Tests;

public interface IMoneyEsGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<Money>> GetAsync();

    Task<Result> AddAsync(Money money);
}

[LogConsistencyProvider(ProviderName = "MoneyEventStore")]
[StorageProvider(ProviderName = "MoneyStore")]
public class MoneyEsGrain : EventSourcingGrain<MoneyEsState>, IMoneyEsGrain
{
    private readonly ILogger<MoneyEsGrain> _logger;

    /// <inheritdoc />
    public MoneyEsGrain(ILogger<MoneyEsGrain> logger)
        : base("Default", "Tests")
    {
        _logger = Guard.Against.Null(logger);
    }

    /// <inheritdoc />
    public Task<Result<Money>> GetAsync()
    {
        return Task.FromResult(Result.Ok(State.Inventory));
    }

    /// <inheritdoc />
    public Task<Result> AddAsync(Money money)
    {
        return PublishAsync(new MoneyEsAddedEvent(Money.FiftyYuan, Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
    }
}

[GenerateSerializer]
public sealed class MoneyEsState
{
    [Id(0)]
    public Money Inventory { get; set; } = Money.Zero;

    public void Apply(MoneyEsAddedEvent evt)
    {
        Inventory += evt.Money;
    }
}

[Immutable]
[GenerateSerializer]
public sealed record MoneyEsAddedEvent : DomainEvent
{
    public MoneyEsAddedEvent(Money money, Guid traceId, DateTimeOffset operatedAt, string operatedBy)
        : base(traceId, operatedAt, operatedBy)
    {
        Money = Guard.Against.Null(money, nameof(money));
    }

    [Id(0)]
    public Money Money { get; }
}
