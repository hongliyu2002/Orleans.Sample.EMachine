using EMachine.Sales.Orleans.States;
using EMachine.Sales.Orleans.Tests.Fixtures;
using FluentAssertions;
using Orleans.TestingHost;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Sales.Orleans.Tests;

[Collection(MoneyCollectionFixture.Name)]
public class MoneyEventSourcingGrainTests
{
    private readonly TestCluster _cluster;
    private readonly ITestOutputHelper _output;

    public MoneyEventSourcingGrainTests(ClusterFixture fixture, ITestOutputHelper output)
    {
        _output = output;
        _cluster = fixture.Cluster;
    }

    [Fact]
    public async Task Can_Get_Money()
    {
        var id = Guid.NewGuid();
        var grain = _cluster.GrainFactory.GetGrain<IMoneyEsGrain>(id);
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Should().Be(Money.Zero);
        _output.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Can_Add_And_Get_Money()
    {
        var id = Guid.NewGuid();
        var grain = _cluster.GrainFactory.GetGrain<IMoneyEsGrain>(id);
        var result = await grain.AddAsync(Money.FiftyYuan);
        result.IsSuccess.Should().Be(true);
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Should().Be(Money.FiftyYuan);
        _output.WriteLine(getResult.ToString());
    }
}
