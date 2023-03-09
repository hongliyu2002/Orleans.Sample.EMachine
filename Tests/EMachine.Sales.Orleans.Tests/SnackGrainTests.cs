using EMachine.Sales.Orleans.Commands;
using EMachine.Sales.Orleans.Tests.Fixtures;
using FluentAssertions;
using Orleans.TestingHost;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Sales.Orleans.Tests;

[Collection(SnackCollectionFixture.Name)]
public class SnackGrainTests
{
    private readonly TestCluster _cluster;
    private readonly ITestOutputHelper _output;

    public SnackGrainTests(ClusterFixture fixture, ITestOutputHelper output)
    {
        _output = output;
        _cluster = fixture.Cluster;
    }

    [Fact]
    public async Task Can_Initialize_And_Get_Snack()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(Guid.NewGuid());
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Orange", Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        var getResult = await grain.GetNameAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Should().Be("Orange");
        _output.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Can_ChangeName_And_Get_Snack()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(Guid.NewGuid());
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Mongo", Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        var changeNameResult = await grain.ChangeNameAsync(new SnackChangeNameCommand("Candy", Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
        changeNameResult.IsSuccess.Should().Be(true);
        var getResult = await grain.GetNameAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Should().Be("Candy");
        _output.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Can_Remove_Snack()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(Guid.NewGuid());
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("BBQ", Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        var removeResult = await grain.RemoveAsync(new SnackRemoveCommand(Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
        removeResult.IsSuccess.Should().Be(true);
        var getResult = await grain.GetNameAsync();
        getResult.IsSuccess.Should().Be(true);
        _output.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Cannot_Reinitialize_Snack()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(Guid.NewGuid());
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Red Tea", Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        var reInitializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Green Tea", Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
        reInitializeResult.IsSuccess.Should().Be(false);
        _output.WriteLine(reInitializeResult.ToString());
        var getResult = await grain.GetNameAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Should().Be("Red Tea");
        _output.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Cannot_Initialize_Snack_When_Deleted()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(Guid.NewGuid());
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Cafe", Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        var removeResult = await grain.RemoveAsync(new SnackRemoveCommand(Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
        removeResult.IsSuccess.Should().Be(true);
        var reInitializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Chocolate", Guid.NewGuid(), DateTimeOffset.UtcNow, "Janet"));
        reInitializeResult.IsSuccess.Should().Be(false);
        _output.WriteLine(reInitializeResult.ToString());
        var getResult = await grain.GetNameAsync();
        getResult.IsSuccess.Should().Be(false);
        _output.WriteLine(getResult.ToString());
    }
}
