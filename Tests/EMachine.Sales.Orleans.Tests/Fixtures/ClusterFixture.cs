using Orleans.TestingHost;

namespace EMachine.Sales.Orleans.Tests.Fixtures;

public class ClusterFixture : IDisposable
{
    public ClusterFixture()
    {
        Cluster = new TestClusterBuilder().AddSiloBuilderConfigurator<TestSiloConfigurator>()
                                          .AddClientBuilderConfigurator<TestClientBuilderConfigurator>()
                                          .Build();
        Cluster.Deploy();
    }

    public TestCluster Cluster { get; }

    public void Dispose()
    {
        Cluster.Dispose();
    }
}
