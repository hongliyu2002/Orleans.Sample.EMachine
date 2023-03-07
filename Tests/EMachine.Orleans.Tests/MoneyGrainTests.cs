using EMachine.Orleans.Shared;
using EMachine.Orleans.Tests.Fixtures;
using FluentAssertions;
using Orleans.TestingHost;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Orleans.Tests;

[Collection(MoneyCollectionFixture.Name)]
public class MoneyGrainTests
{
    private readonly TestCluster _cluster;
    private readonly ITestOutputHelper _testOutputHelper;

    public MoneyGrainTests(ClusterFixture fixture, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _cluster = fixture.Cluster;
    }

    public IMoneyGrain MoneyGrain => _cluster.GrainFactory.GetGrain<IMoneyGrain>(Guid.Empty);

    [Fact]
    public async Task Can_Set_And_Get_Money()
    {
        var money1 = new Money(1, 2, 3, 4, 5, 6, 7);
        await MoneyGrain.Set(money1);
        var money = await MoneyGrain.Get();
        money.Should().Be(money1);
    }

    [Fact]
    public async Task Can_Get_Money_When_Initialized()
    {
        var action = async () =>
                     {
                         var money = await MoneyGrain.Get();
                         _testOutputHelper.WriteLine(money.ToString());
                     };
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Can_Plus_Money()
    {
        var money1 = new Money(1, 2, 3, 4, 5, 6, 7);
        var money2 = new Money(1, 2, 3, 4, 5, 6, 7);
        await MoneyGrain.Set(money1);
        await MoneyGrain.Plus(money2);
        var money = await MoneyGrain.Get();
        money.Yuan1.Should().Be(2);
        money.Yuan2.Should().Be(4);
        money.Yuan5.Should().Be(6);
        money.Yuan10.Should().Be(8);
        money.Yuan20.Should().Be(10);
        money.Yuan50.Should().Be(12);
        money.Yuan100.Should().Be(14);
    }

    [Fact]
    public async Task Can_Minus_Money()
    {
        var money1 = new Money(1, 2, 3, 4, 5, 6, 7);
        var money2 = new Money(1, 2, 3, 4, 5, 6, 7);
        await MoneyGrain.Set(money1);
        await MoneyGrain.Minus(money2);
        var money = await MoneyGrain.Get();
        money.Should().Be(Money.Zero);
    }

    [Fact]
    public async Task Cannot_Minus_Money_With_Invalid_Value()
    {
        var action = async () =>
                     {
                         var money1 = new Money(1, 2, 3, 4, 5, 6, 7);
                         var money2 = new Money(7, 6, 5, 4, 3, 2, 1);
                         await MoneyGrain.Set(money1);
                         await MoneyGrain.Minus(money2);
                     };
        await action.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Can_Multiply_With_Multiplier()
    {
        var money1 = new Money(1, 2, 3, 4, 5, 6, 7);
        await MoneyGrain.Set(money1);
        await MoneyGrain.Multiply(10);
        var money = await MoneyGrain.Get();
        money.Should().Be(new Money(10, 20, 30, 40, 50, 60, 70));
    }

    [Fact]
    public async Task Cannot_Multiply_With_Invalid_Value()
    {
        var action = async () =>
                     {
                         var money1 = new Money(1, 2, 3, 4, 5, 6, 7);
                         await MoneyGrain.Set(money1);
                         await MoneyGrain.Multiply(-1);
                     };
        await action.Should().ThrowAsync<ArgumentException>();
    }
}
