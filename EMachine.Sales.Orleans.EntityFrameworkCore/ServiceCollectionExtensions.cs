using Fluxera.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EMachine.Sales.Orleans.EntityFrameworkCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSalesDbContextPool(this IServiceCollection services, SalesDbOptions options)
    {
        return services.AddDbContextPool<SalesDbContext>(dbContext =>
                                                         {
                                                             if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out var connectionString))
                                                             {
                                                                 switch (options.DbProvider)
                                                                 {
                                                                     case SalesDbProvider.SqlServer:
                                                                         UseSqlServer(dbContext, connectionString, options);
                                                                         break;
                                                                     case SalesDbProvider.PostgreSQL:
                                                                         break;
                                                                     case SalesDbProvider.MySQL:
                                                                         break;
                                                                     case SalesDbProvider.Sqlite:
                                                                         UseSqlite(dbContext, connectionString, options);
                                                                         break;
                                                                     default:
                                                                         throw new ArgumentOutOfRangeException(nameof(options.DbProvider), "Database provider does not support.");
                                                                 }
                                                             }
                                                         });
    }

    private static void UseSqlite(DbContextOptionsBuilder builder, string connectionString, SalesDbOptions options)
    {
        builder.UseSqlite(connectionString, dbContext =>
                                            {
                                                if (options.MigrationsHistoryTable.IsNotNullOrEmpty())
                                                {
                                                    var schemaAndName = options.MigrationsHistoryTable!.Trim().Split(".");
                                                    if (schemaAndName.Length >= 1)
                                                    {
                                                        dbContext.MigrationsHistoryTable(schemaAndName[1], schemaAndName[0]);
                                                    }
                                                    else
                                                    {
                                                        dbContext.MigrationsHistoryTable(schemaAndName[0]);
                                                    }
                                                }
                                                dbContext.UseQuerySplittingBehavior(options.QuerySplittingBehavior);
                                            });
    }

    private static void UseSqlServer(DbContextOptionsBuilder builder, string connectionString, SalesDbOptions options)
    {
        builder.UseSqlServer(connectionString, dbContext =>
                                               {
                                                   if (options.MigrationsHistoryTable.IsNotNullOrEmpty())
                                                   {
                                                       var schemaAndName = options.MigrationsHistoryTable!.Trim().Split(".");
                                                       if (schemaAndName.Length >= 1)
                                                       {
                                                           dbContext.MigrationsHistoryTable(schemaAndName[1], schemaAndName[0]);
                                                       }
                                                       else
                                                       {
                                                           dbContext.MigrationsHistoryTable(schemaAndName[0]);
                                                       }
                                                   }
                                                   dbContext.EnableRetryOnFailure(options.MaxRetry, TimeSpan.FromMilliseconds(options.MaxRetryDelay), new[] { -1000 });
                                                   dbContext.UseQuerySplittingBehavior(options.QuerySplittingBehavior);
                                               });
    }
}
