using System.Reflection;
using Fluxera.Guards;
using Fluxera.Utilities.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EMachine.Persistence.EntityFrameworkCore;

/// <summary>
///     Extension methods for the <see cref="IServiceCollection" /> type.
/// </summary>
[PublicAPI]
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Registers the given context as a service in the <see cref="IServiceCollection" />.
    /// </summary>
    public static IServiceCollection AddDbContext(this IServiceCollection services, Type dbContextType, Action<DbContextOptionsBuilder>? optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
                                                  ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
    {
        Guard.Against.Null(services);
        Guard.Against.Null(dbContextType);
        Guard.Against.False(dbContextType.IsAssignableTo<DbContext>(), "The type to register must inherit from DbContext");
        var methodInfo = typeof(EntityFrameworkServiceCollectionExtensions).GetRuntimeMethods()
                                                                           .Where(x => x.Name == "AddDbContext")
                                                                           .Where(x => x.IsGenericMethod)
                                                                           .Where(x => x.GetGenericArguments().Length == 1)
                                                                           .Where(x => x.GetParameters().Length == 4)
                                                                           .Where(x => x.GetParameters()[1].ParameterType == typeof(Action<>))
                                                                           .Where(x => x.GetParameters()[2].ParameterType == typeof(ServiceLifetime))
                                                                           .Single(x => x.GetParameters()[3].ParameterType == typeof(ServiceLifetime));
        methodInfo.MakeGenericMethod(dbContextType)
       .Invoke(null, new object?[]
                     {
                         services,
                         optionsAction,
                         contextLifetime,
                         optionsLifetime
                     });
        return services;
    }
    
    /// <summary>
    ///     Registers the given context as a service in the <see cref="IServiceCollection" />.
    /// </summary>
    public static IServiceCollection AddDbContextPool(this IServiceCollection services, Type dbContextType, Action<DbContextOptionsBuilder>? optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
                                                  ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
    {
        Guard.Against.Null(services);
        Guard.Against.Null(dbContextType);
        Guard.Against.False(dbContextType.IsAssignableTo<DbContext>(), "The type to register must inherit from DbContext");
        var methodInfo = typeof(EntityFrameworkServiceCollectionExtensions).GetRuntimeMethods()
                                                                           .Where(x => x.Name == "AddDbContext")
                                                                           .Where(x => x.IsGenericMethod)
                                                                           .Where(x => x.GetGenericArguments().Length == 1)
                                                                           .Where(x => x.GetParameters().Length == 4)
                                                                           .Where(x => x.GetParameters()[1].ParameterType == typeof(Action<>))
                                                                           .Where(x => x.GetParameters()[2].ParameterType == typeof(ServiceLifetime))
                                                                           .Single(x => x.GetParameters()[3].ParameterType == typeof(ServiceLifetime));
        methodInfo.MakeGenericMethod(dbContextType)
                  .Invoke(null, new object?[]
                                {
                                    services,
                                    optionsAction,
                                    contextLifetime,
                                    optionsLifetime
                                });
        return services;
    }
}
