﻿using EMachine.Orleans.Shared;
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

    Task<Result<bool>> AddAsync(Money money);
}

[LogConsistencyProvider(ProviderName = "MoneyEventStore")]
[StorageProvider(ProviderName = "MoneyStore")]
public class MoneyEsGrain : EventSourcingGrain<MoneyEsState>, IMoneyEsGrain
{
    /// <inheritdoc />
    public MoneyEsGrain()
        : base(Constants.StreamProviderName, "Tests")
    {
    }

    /// <inheritdoc />
    public Task<Result<Money>> GetAsync()
    {
        return Task.FromResult(Result.Ok(State.Inventory));
    }

    /// <inheritdoc />
    public Task<Result<bool>> AddAsync(Money money)
    {
        return PublishAsync(new MoneyEsAddedEvent(Money.FiftyYuan, Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo", 1));
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
    public MoneyEsAddedEvent(Money money, Guid traceId, DateTimeOffset operatedAt, string operatedBy, int version)
        : base(traceId, operatedAt, operatedBy, version)
    {
        Money = Guard.Against.Null(money, nameof(money));
    }

    [Id(0)]
    public Money Money { get; }
}
