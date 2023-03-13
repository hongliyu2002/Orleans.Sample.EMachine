using EMachine.Sales.EntityFrameworkCore.Contexts;
using Fluxera.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EMachine.Sales.EntityFrameworkCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContextPoolForSales(this IServiceCollection services, EfCoreDatabaseOptions options)
    {
        return services.AddDbContextPool<SalesDbContext>(dbContext =>
                                                         {
                                                             if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out var connectionString))
                                                             {
                                                                 switch (options.DatabaseProvider)
                                                                 {
                                                                     case EfCoreDatabaseProvider.Sqlite:
                                                                         UseSqlite(dbContext, connectionString, options);
                                                                         break;
                                                                     case EfCoreDatabaseProvider.SqlServer:
                                                                         UseSqlServer(dbContext, connectionString, options);
                                                                         break;
                                                                     case EfCoreDatabaseProvider.PostgreSql:
                                                                         break;
                                                                     case EfCoreDatabaseProvider.MySql:
                                                                         break;
                                                                     default:
                                                                         throw new ArgumentOutOfRangeException(nameof(options.DatabaseProvider), "Database provider does not support.");
                                                                 }
                                                             }
                                                         });
    }

    private static void UseSqlite(DbContextOptionsBuilder builder, string connectionString, EfCoreDatabaseOptions options)
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

    private static void UseSqlServer(DbContextOptionsBuilder builder, string connectionString, EfCoreDatabaseOptions options)
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
