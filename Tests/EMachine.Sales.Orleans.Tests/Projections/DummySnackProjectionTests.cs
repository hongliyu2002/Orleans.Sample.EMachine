using EMachine.Orleans.Shared.Events;
using EMachine.Sales.EntityFrameworkCore.Contexts;
using EMachine.Sales.Orleans.Events;
using EMachine.Sales.Orleans.Tests.Fixtures;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Providers.Streams.Common;
using Orleans.Runtime;
using Orleans.TestingHost;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Sales.Orleans.Tests.Projections;

[Collection(TestCollectionFixture.Name)]
public class DummySnackProjectionTests
{
    private readonly TestCluster _cluster;
    private readonly SalesDbContext _dbContext;
    private readonly ITestOutputHelper _output;

    public DummySnackProjectionTests(ClusterFixture fixture, ITestOutputHelper output)
    {
        _output = output;
        _cluster = fixture.Cluster;
        _dbContext = _cluster.ServiceProvider.GetRequiredService<SalesDbContext>();
        // _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
    }

    [Fact]
    public async Task Should_Get_IdGenerator()
    {
        var id = Guid.NewGuid();
        var streamProvider = _cluster.Client.GetStreamProvider(Constants.StreamProviderName);
        var stream = streamProvider.GetStream<DomainEvent>(StreamId.Create(Constants.SnackNamespace, id));
        await stream.OnNextAsync(new SnackInitializedEvent(id, "Fuck", Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo", 1), new EventSequenceToken(1));
        await Task.Delay(2000);
        var snack = await _dbContext.Snacks.FindAsync(id);
        snack.Should().NotBeNull();
        snack!.Name.Should().Be("Fuck");
        _output.WriteLine(snack.ToString());
    }

    [Fact]
    public void Should_Be_SN()
    {
        var sn = new EventSequenceTokenV2(1);
        sn.SequenceNumber.Should().Be(1);
    }
}
