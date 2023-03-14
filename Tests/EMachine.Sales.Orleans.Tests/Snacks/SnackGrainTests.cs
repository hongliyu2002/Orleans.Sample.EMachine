using EMachine.Sales.Orleans.Commands;
using EMachine.Sales.Orleans.Tests.Fixtures;
using FluentAssertions;
using Fluxera.Extensions.Hosting.Modules.UnitTesting;
using Orleans.TestingHost;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Sales.Orleans.Tests;

[Collection(TestCollectionFixture.Name)]
public class SnackGrainTests : StartupModuleTestBase<SalesOrleansModule>
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
        var id = Guid.NewGuid();
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(id);
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Orange", Guid.NewGuid(), DateTimeOffset.UtcNow, "Can_Initialize_And_Get_Snack"));
        initializeResult.IsSuccess.Should().Be(true);
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Id.Should().Be(id);
        getResult.Value.Name.Should().Be("Orange");
        _output.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Cannot_Reinitialize_Snack()
    {
        var id = Guid.NewGuid();
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(id);
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Red Tea", Guid.NewGuid(), DateTimeOffset.UtcNow, "Cannot_Reinitialize_Snack"));
        initializeResult.IsSuccess.Should().Be(true);
        var reInitializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Green Tea", Guid.NewGuid(), DateTimeOffset.UtcNow, "Cannot_Reinitialize_Snack"));
        reInitializeResult.IsSuccess.Should().Be(false);
        _output.WriteLine(reInitializeResult.ToString());
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Id.Should().Be(id);
        getResult.Value.Name.Should().Be("Red Tea");
        _output.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Can_Remove_Snack()
    {
        var id = Guid.NewGuid();
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(id);
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("BBQ", Guid.NewGuid(), DateTimeOffset.UtcNow, "Can_Remove_Snack"));
        initializeResult.IsSuccess.Should().Be(true);
        var removeResult = await grain.RemoveAsync(new SnackRemoveCommand(id, DateTimeOffset.UtcNow, "Can_Remove_Snack"));
        removeResult.IsSuccess.Should().Be(true);
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Id.Should().Be(id);
        getResult.Value.Name.Should().Be("BBQ");
        getResult.Value.IsDeleted.Should().Be(true);
        _output.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Cannot_Reinitialize_Snack_When_Removed()
    {
        var id = Guid.NewGuid();
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(id);
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Cookies", Guid.NewGuid(), DateTimeOffset.UtcNow, "Cannot_Reinitialize_Snack_When_Removed"));
        initializeResult.IsSuccess.Should().Be(true);
        var removeResult = await grain.RemoveAsync(new SnackRemoveCommand(id, DateTimeOffset.UtcNow, "Cannot_Reinitialize_Snack_When_Removed"));
        removeResult.IsSuccess.Should().Be(true);
        var reInitializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Nuts", Guid.NewGuid(), DateTimeOffset.UtcNow, "Cannot_Reinitialize_Snack_When_Removed"));
        reInitializeResult.IsSuccess.Should().Be(false);
        _output.WriteLine(reInitializeResult.ToString());
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Id.Should().Be(id);
        getResult.Value.Name.Should().Be("Cookies");
        getResult.Value.IsDeleted.Should().Be(true);
        _output.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Can_ChangeName_And_Get_Snack()
    {
        var id = Guid.NewGuid();
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(id);
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Mongo", Guid.NewGuid(), DateTimeOffset.UtcNow, "Can_ChangeName_And_Get_Snack"));
        initializeResult.IsSuccess.Should().Be(true);
        var changeNameResult = await grain.ChangeNameAsync(new SnackChangeNameCommand("Candy", Guid.NewGuid(), DateTimeOffset.UtcNow, "Can_ChangeName_And_Get_Snack"));
        changeNameResult.IsSuccess.Should().Be(true);
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Id.Should().Be(id);
        getResult.Value.Name.Should().Be("Candy");
        _output.WriteLine(getResult.ToString());
    }
}
