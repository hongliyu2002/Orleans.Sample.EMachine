using EMachine.Orleans.Shared;
using EMachine.Orleans.Tests.Fixtures;
using FluentAssertions;
using Orleans.TestingHost;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Orleans.Tests;

[Collection(MoneyCollectionFixture.Name)]
public class MoneyEventSourcingGrainTests
{
    private readonly TestCluster _cluster;
    private readonly ITestOutputHelper _testOutputHelper;

    public MoneyEventSourcingGrainTests(ClusterFixture fixture, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
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
        _testOutputHelper.WriteLine(getResult.ToString());
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
        _testOutputHelper.WriteLine(getResult.ToString());
    }
}
