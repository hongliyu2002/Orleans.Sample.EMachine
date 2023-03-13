using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;

namespace EMachine.Orleans.Server;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrleansServer(this IServiceCollection services, ServerOptions options)
    {
        return services.AddOrleans(builder =>
                                   {
                                       builder.Configure<ClusterOptions>(cluster =>
                                                                         {
                                                                             cluster.ServiceId = options.Cluster.ServiceId;
                                                                             cluster.ClusterId = options.Cluster.ClusterId;
                                                                         });
                                       builder.Configure<global::Orleans.Configuration.EndpointOptions>(endpoint =>
                                                                                                        {
                                                                                                            endpoint.AdvertisedIPAddress = options.Endpoint.ClusterIPAddress;
                                                                                                            endpoint.SiloPort = options.Endpoint.SiloPort;
                                                                                                            endpoint.SiloListeningEndpoint = options.Endpoint.SiloListeningEndpoint;
                                                                                                            endpoint.GatewayPort = options.Endpoint.GatewayPort;
                                                                                                            endpoint.GatewayListeningEndpoint = options.Endpoint.GatewayListeningEndpoint;
                                                                                                        });
                                       builder.Configure<SiloOptions>(silo =>
                                                                      {
                                                                          silo.SiloName = options.SiloName;
                                                                      });
                                   });
    }
}
