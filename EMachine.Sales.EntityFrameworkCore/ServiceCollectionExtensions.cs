using EMachine.Sales.EntityFrameworkCore.Contexts;
using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.DependencyInjection;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EMachine.Sales.EntityFrameworkCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContextPoolForSales(this IServiceCollection services)
    {
        return services.AddDbContextPool<SalesDbContext>(dbContext =>
                                                         {
                                                             var efOptions = services.GetOptions<EfCoreDatabaseOptions>();
                                                             efOptions.ConnectionStrings = services.GetObject<ConnectionStrings>();
                                                             if (!efOptions.ConnectionStrings.TryGetValue(efOptions.ConnectionStringName, out var connectionString))
                                                             {
                                                                 return;
                                                             }
                                                             switch (efOptions.DatabaseProvider)
                                                             {
                                                                 case EfDatabaseProvider.Sqlite:
                                                                     dbContext.UseSqlite(connectionString, options =>
                                                                                                           {
                                                                                                               if (efOptions.MigrationsHistoryTable.IsNotNullOrEmpty())
                                                                                                               {
                                                                                                                   var schemaAndName = efOptions.MigrationsHistoryTable!.Trim().Split(".");
                                                                                                                   if (schemaAndName.Length >= 1)
                                                                                                                   {
                                                                                                                       options.MigrationsHistoryTable(schemaAndName[1], schemaAndName[0]);
                                                                                                                   }
                                                                                                                   else
                                                                                                                   {
                                                                                                                       options.MigrationsHistoryTable(schemaAndName[0]);
                                                                                                                   }
                                                                                                               }
                                                                                                               options.UseQuerySplittingBehavior(efOptions.QuerySplittingBehavior);
                                                                                                           });
                                                                     break;
                                                                 case EfDatabaseProvider.SqlServer:
                                                                     dbContext.UseSqlServer(connectionString, options =>
                                                                                                              {
                                                                                                                  if (efOptions.MigrationsHistoryTable.IsNotNullOrEmpty())
                                                                                                                  {
                                                                                                                      var schemaAndName = efOptions.MigrationsHistoryTable!.Trim().Split(".");
                                                                                                                      if (schemaAndName.Length >= 1)
                                                                                                                      {
                                                                                                                          options.MigrationsHistoryTable(schemaAndName[1], schemaAndName[0]);
                                                                                                                      }
                                                                                                                      else
                                                                                                                      {
                                                                                                                          options.MigrationsHistoryTable(schemaAndName[0]);
                                                                                                                      }
                                                                                                                  }
                                                                                                                  options.EnableRetryOnFailure(efOptions.MaxRetry, TimeSpan.FromMilliseconds(efOptions.MaxRetryDelay), new[] { -1000 });
                                                                                                                  options.UseQuerySplittingBehavior(efOptions.QuerySplittingBehavior);
                                                                                                              });
                                                                     break;
                                                                 case EfDatabaseProvider.PostgreSql:
                                                                     break;
                                                                 case EfDatabaseProvider.MySql:
                                                                     break;
                                                                 default:
                                                                     throw new ArgumentOutOfRangeException();
                                                             }
                                                         });
    }
}
