using System.Reflection;
using EMachine.Orleans;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.AspNetCore.HealthChecks;
using Fluxera.Extensions.Hosting.Modules.Serilog;
using Fluxera.Extensions.Hosting.Plugins;
using JetBrains.Annotations;
using Serilog;
using Serilog.Extensions.Logging;

namespace EMachine.Sales.Hosting;

[PublicAPI]
[UsedImplicitly]
internal sealed class SalesApplicationHost : WebApplicationHost<SalesHostingModule>
{
    /// <inheritdoc />
    protected override void ConfigureApplicationHostEvents(ApplicationHostEvents applicationHostEvents)
    {
        // base.ConfigureApplicationHostEvents(applicationHostEvents);
        // applicationHostEvents.HostCreating += (sender, args) =>
        //                                       {
        //                                       };
        // applicationHostEvents.HostCreated += (sender, args) =>
        //                                      {
        //                                      };
        // applicationHostEvents.HostCreationFailed += (sender, args) =>
        //                                             {
        //                                             };
    }

    /// <inheritdoc />
    protected override ILoggerFactory CreateBootstrapperLoggerFactory(IConfiguration configuration)
    {
        var logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).Enrich.FromLogContext().CreateBootstrapLogger();
        return new SerilogLoggerFactory(logger);
    }

    /// <inheritdoc />
    protected override void ConfigureHostBuilder(IHostBuilder builder)
    {
        base.ConfigureHostBuilder(builder);
        // // Use Autofac as default container.
        // builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.ConfigureAppConfiguration(configuration =>
                                          {
                                              configuration.AddUserSecrets(Assembly.GetExecutingAssembly());
                                          });
        builder.AddSerilogLogging((context, loggerConfig) =>
                                  {
                                      loggerConfig.ReadFrom.Configuration(context.Configuration);
                                  });
        builder.AddOrleans();
    }

    /// <inheritdoc />
    protected override void ConfigureApplicationPlugins(IPluginConfigurationContext context)
    {
        base.ConfigureApplicationPlugins(context);
        context.AddPlugin<SerilogModule>();
        context.AddPlugin<HealthChecksEndpointsModule>();
    }
}
