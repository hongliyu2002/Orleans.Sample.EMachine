using System.Collections.Immutable;
using EMachine.Sales.Orleans.Commands;
using EMachine.Sales.Orleans.States;
using EMachine.Sales.Orleans.Tests.Fixtures;
using FluentAssertions;
using Orleans.TestingHost;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Sales.Orleans.Tests;

[Collection(SnackMachineCollectionFixture.Name)]
public class SnackMachineGrainTests : IClassFixture<SnackMachineFixture>
{
    private readonly TestCluster _cluster;
    private readonly ITestOutputHelper _output;

    public SnackMachineGrainTests(ClusterFixture fixture, ITestOutputHelper output)
    {
        _output = output;
        _cluster = fixture.Cluster;
    }

    [Fact]
    public async Task Can_Initialize_And_Get_SnackMachine()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackMachineGrain>(Guid.NewGuid());
        var cafeSnackId = new Guid("ae9e8d38-8289-47fe-8084-99df2b894556");
        var chocolateSnackId = new Guid("23697d49-75f1-4e3c-aa0d-5a98cf3ad122");
        var slots = ImmutableList.Create(new Slot(0), new Slot(1, new SnackPile(cafeSnackId, 10, 5)), new Slot(2), new Slot(3, new SnackPile(chocolateSnackId, 20, 8)));
        var initializeResult = await grain.InitializeAsync(new SnackMachineInitializeCommand(Money.OneHundredYuan, slots, Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        await Task.Delay(1000);
        var getResult = await grain.GetSlotsAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Should().BeEquivalentTo(slots);
        await Task.Delay(1000);
        _output.WriteLine(getResult.ToString());
    }
    //
    // [Fact]
    // public async Task Can_ChangeName_And_Get_Snack()
    // {
    //     var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(Guid.NewGuid());
    //     var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Mongo", Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
    //     initializeResult.IsSuccess.Should().Be(true);
    //     var changeNameResult = await grain.ChangeNameAsync(new SnackChangeNameCommand("Candy", Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
    //     changeNameResult.IsSuccess.Should().Be(true);
    //     await Task.Delay(1000);
    //     var getResult = await grain.GetNameAsync();
    //     getResult.IsSuccess.Should().Be(true);
    //     getResult.Value.Should().Be("Candy");
    //     await Task.Delay(1000);
    //     _output.WriteLine(getResult.ToString());
    // }
    //
    // [Fact]
    // public async Task Can_Remove_Snack()
    // {
    //     var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(Guid.NewGuid());
    //     var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("BBQ", Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
    //     initializeResult.IsSuccess.Should().Be(true);
    //     var removeResult = await grain.RemoveAsync(new SnackRemoveCommand(Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
    //     removeResult.IsSuccess.Should().Be(true);
    //     await Task.Delay(1000);
    //     var getResult = await grain.GetNameAsync();
    //     getResult.IsSuccess.Should().Be(true);
    //     await Task.Delay(1000);
    //     _output.WriteLine(getResult.ToString());
    // }
    //
    // [Fact]
    // public async Task Cannot_Reinitialize_Snack()
    // {
    //     var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(Guid.NewGuid());
    //     var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Red Tea", Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
    //     initializeResult.IsSuccess.Should().Be(true);
    //     var reInitializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Green Tea", Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
    //     reInitializeResult.IsSuccess.Should().Be(false);
    //     _output.WriteLine(reInitializeResult.ToString());
    //     await Task.Delay(1000);
    //     var getResult = await grain.GetNameAsync();
    //     getResult.IsSuccess.Should().Be(true);
    //     getResult.Value.Should().Be("Red Tea");
    //     await Task.Delay(1000);
    //     _output.WriteLine(getResult.ToString());
    // }
    //
    // [Fact]
    // public async Task Cannot_Initialize_Snack_When_Deleted()
    // {
    //     var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(Guid.NewGuid());
    //     var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Cafe", Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
    //     initializeResult.IsSuccess.Should().Be(true);
    //     var removeResult = await grain.RemoveAsync(new SnackRemoveCommand(Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
    //     removeResult.IsSuccess.Should().Be(true);
    //     var reInitializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Chocolate", Guid.NewGuid(), DateTimeOffset.UtcNow, "Janet"));
    //     reInitializeResult.IsSuccess.Should().Be(false);
    //     _output.WriteLine(reInitializeResult.ToString());
    //     await Task.Delay(1000);
    //     var getResult = await grain.GetNameAsync();
    //     getResult.IsSuccess.Should().Be(true);
    //     await Task.Delay(1000);
    //     _output.WriteLine(getResult.ToString());
    // }
}
