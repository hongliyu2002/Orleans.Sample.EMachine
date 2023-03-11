using EMachine.Sales.Orleans.Commands;
using EMachine.Sales.Orleans.Tests.Fixtures;
using FluentAssertions;
using Orleans.TestingHost;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Sales.Orleans.Tests;

[Collection(SnackRepoCollectionFixture.Name)]
public class SnackCrudRepoGainTests : IClassFixture<SnackCrudRepoFixture>
{
    private readonly TestCluster _cluster;
    private readonly ITestOutputHelper _output;

    public SnackCrudRepoGainTests(ClusterFixture fixture, ITestOutputHelper output)
    {
        _cluster = fixture.Cluster;
        _output = output;
    }

    [Fact]
    public async Task Can_Create_Snack()
    {
        var id = Guid.NewGuid();
        var repoGrain = _cluster.GrainFactory.GetGrain<ISnackCrudRepoGrain>(Guid.Empty);
        var createResult = await repoGrain.CreateAsync(new SnackRepoCreateCommand(id, "Apple", Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
        createResult.IsSuccess.Should().Be(true);
        _output.WriteLine(createResult.ToString());
        await Task.Delay(1000);
        var getResult = await repoGrain.GetAsync(new SnackRepoGetCommand(id, Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.GetPrimaryKey().Should().Be(id);
        var result = await getResult.Value.GetAsync();
        result.IsSuccess.Should().Be(true);
        result.Value.Id.Should().Be(id);
        result.Value.Name.Should().Be("Apple");
        await Task.Delay(1000);
        _output.WriteLine(result.ToString());
    }

    [Fact]
    public async Task Can_Delete_Snack()
    {
        var id = Guid.NewGuid();
        var repoGrain = _cluster.GrainFactory.GetGrain<ISnackCrudRepoGrain>(Guid.Empty);
        var createResult = await repoGrain.CreateAsync(new SnackRepoCreateCommand(id, "Lemon", Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
        createResult.IsSuccess.Should().Be(true);
        _output.WriteLine(createResult.ToString());
        var deleteResult = await repoGrain.DeleteAsync(new SnackRepoDeleteCommand(id, Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
        deleteResult.IsSuccess.Should().Be(true);
        _output.WriteLine(deleteResult.ToString());
        await Task.Delay(1000);
        var getResult = await repoGrain.GetAsync(new SnackRepoGetCommand(id, Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.GetPrimaryKey().Should().Be(id);
        var result = await getResult.Value.GetAsync();
        result.IsSuccess.Should().Be(true);
        result.Value.Id.Should().Be(id);
        result.Value.Name.Should().Be("Lemon");
        result.Value.IsDeleted.Should().BeTrue();
        await Task.Delay(1000);
        _output.WriteLine(result.ToString());
    }

    [Fact]
    public async Task Can_Get_Snack()
    {
        var id = new Guid("ae9e8d38-8289-47fe-8084-99df2b894556");
        var repoGrain = _cluster.GrainFactory.GetGrain<ISnackCrudRepoGrain>(Guid.Empty);
        var getResult = await repoGrain.GetAsync(new SnackRepoGetCommand(id, Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.GetPrimaryKey().Should().Be(id);
        var result = await getResult.Value.GetAsync();
        result.IsSuccess.Should().Be(true);
        result.Value.Id.Should().Be(id);
        result.Value.Name.Should().Be("Cafe");
        await Task.Delay(1000);
        _output.WriteLine(result.ToString());
    }

    [Fact]
    public async Task Can_Get_Snacks()
    {
        var repoGrain = _cluster.GrainFactory.GetGrain<ISnackCrudRepoGrain>(Guid.Empty);
        var getResult = await repoGrain.GetMultipleAsync(new SnackRepoGetManyCommand(new[]
                                                                                         {
                                                                                             new Guid("23697d49-75f1-4e3c-aa0d-5a98cf3ad122"),
                                                                                             new Guid("5b4103f4-7d90-4680-afc1-70dc48b96629"),
                                                                                             new Guid("ad63bc13-5075-47d7-8525-b32b52352192")
                                                                                         }, Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.Count.Should().Be(3);
        getResult.Value.ForEach(OnNext);
        async void OnNext(ISnackGrain grain)
        {
            var result = await grain.GetAsync();
            result.IsSuccess.Should().Be(true);
            _output.WriteLine(result.ToString());
        }
    }
}
