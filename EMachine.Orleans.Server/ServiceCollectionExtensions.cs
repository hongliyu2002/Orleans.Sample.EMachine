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
                                                                             cluster.ServiceId = options.ServiceId;
                                                                             cluster.ClusterId = options.ClusterId;
                                                                         });
                                       builder.Configure<EndpointOptions>(endpoint =>
                                                                          {
                                                                              endpoint.AdvertisedIPAddress = options.AdvertisedIPAddress;
                                                                              endpoint.SiloPort = options.SiloPort;
                                                                              endpoint.GatewayPort = options.GatewayPort;
                                                                              endpoint.SiloListeningEndpoint = options.SiloListeningEndpoint;
                                                                              endpoint.GatewayListeningEndpoint = options.GatewayListeningEndpoint;
                                                                          });
                                       builder.Configure<SiloOptions>(silo =>
                                                                      {
                                                                          silo.SiloName = options.SiloName;
                                                                      });
                                       switch (options.BroadcastChannelNames.Length)
                                       {
                                           case > 0:
                                           {
                                               foreach (var name in options.BroadcastChannelNames)
                                               {
                                                   builder.AddBroadcastChannel(name);
                                               }
                                               break;
                                           }
                                           default:
                                               builder.AddBroadcastChannel("eMachine");
                                               break;
                                       }
                                   });
    }
}
