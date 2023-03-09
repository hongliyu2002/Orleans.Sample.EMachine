using EMachine.Sales.Orleans.Commands;
using EMachine.Sales.Orleans.Tests.Fixtures;
using FluentAssertions;
using Orleans.TestingHost;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Sales.Orleans.Tests;

[Collection(SnackRepositoryCollectionFixture.Name)]
public class SnackRepositoryGainTests : IClassFixture<SnackRepositoryFixture>
{
    private readonly TestCluster _cluster;
    private readonly ITestOutputHelper _output;

    public SnackRepositoryGainTests(ClusterFixture fixture, ITestOutputHelper output)
    {
        _output = output;
        _cluster = fixture.Cluster;
    }

    [Fact]
    public async Task Can_Create_Snack()
    {
        var id = Guid.NewGuid();
        var grain = _cluster.GrainFactory.GetGrain<ISnackCrudRepoGrain>(Guid.Empty);
        var createResult = await grain.CreateAsync(new SnackCrudRepoCreateOneCommand(id, "Apple", Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
        createResult.IsSuccess.Should().Be(true);
        _output.WriteLine(createResult.ToString());
        
        var getResult = await grain.GetAsync(new SnackCrudRepoGetOneCommand(id, Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.GetPrimaryKey().Should().Be(id);
        var result = await getResult.Value.GetNameAsync();
        result.IsSuccess.Should().Be(true);
        result.Value.Should().Be("Apple");
        _output.WriteLine(result.ToString());
    }

    [Fact]
    public async Task Can_Delete_Snack()
    {
        var id = Guid.NewGuid();
        var grain = _cluster.GrainFactory.GetGrain<ISnackCrudRepoGrain>(Guid.Empty);
        var createResult = await grain.CreateAsync(new SnackCrudRepoCreateOneCommand(id, "Lemon", Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
        createResult.IsSuccess.Should().Be(true);
        _output.WriteLine(createResult.ToString());
        
        var deleteResult = await grain.DeleteAsync(new SnackCrudRepoDeleteOneCommand(id, Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
        deleteResult.IsSuccess.Should().Be(true);
        _output.WriteLine(deleteResult.ToString());
        
        var getResult = await grain.GetAsync(new SnackCrudRepoGetOneCommand(id, Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.GetPrimaryKey().Should().Be(id);
        var result = await getResult.Value.GetNameAsync();
        result.IsSuccess.Should().Be(false);
        _output.WriteLine(result.ToString());
    }

    [Fact]
    public async Task Can_Get_Snack()
    {
        var id = new Guid("ae9e8d38-8289-47fe-8084-99df2b894556");
        var grain = _cluster.GrainFactory.GetGrain<ISnackCrudRepoGrain>(Guid.Empty);
        var getResult = await grain.GetAsync(new SnackCrudRepoGetOneCommand(id, Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.GetPrimaryKey().Should().Be(id);
        var result = await getResult.Value.GetNameAsync();
        result.Value.Should().Be("Cafe");
        _output.WriteLine(result.ToString());
    }

    [Fact]
    public async Task Can_Get_Snacks()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackCrudRepoGrain>(Guid.Empty);
        var getResult = await grain.GetMultipleAsync(new SnackCrudRepoGetManyCommand(new[]
                                                                                       {
                                                                                           new("23697d49-75f1-4e3c-aa0d-5a98cf3ad122"),
                                                                                           new Guid("5b4103f4-7d90-4680-afc1-70dc48b96629"),
                                                                                           new Guid("ad63bc13-5075-47d7-8525-b32b52352192")
                                                                                       }, Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.Count.Should().Be(3);
        getResult.Value.ForEach(x => _output.WriteLine(x.ToString()));
    }
}
