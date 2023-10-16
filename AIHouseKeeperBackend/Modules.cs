using System.Reflection;
using AIHouseKeeperBackend.DependencyInjections;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace AIHouseKeeperBackend;

public static class Modules
{
    public static void BuildServices(IServiceCollection b, params Assembly[] additionalAssemblies)
    {
        var entryAssembly = Assembly.GetCallingAssembly();
        var executingAssembly = Assembly.GetExecutingAssembly();

        var hashSet = new HashSet<Assembly>
        {
            entryAssembly, executingAssembly
        };
        foreach (var additionalAssembly in additionalAssemblies)
        {
            hashSet.Add(additionalAssembly);
        }
        b.Scan(
            scan => scan
                .FromAssemblies(hashSet)
                .AddClasses(classes => classes.AssignableTo<ISingletonService>())
                .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                .AsMatchingInterface()
                .WithSingletonLifetime());

        b.Scan(
            scan => scan
                .FromAssemblies(hashSet)
                .AddClasses(classes => classes.AssignableTo<IScopedService>())
                .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                .AsMatchingInterface()
                .WithScopedLifetime());
    }

    public static void BuildConfigs(IServiceCollection b, params Assembly[] additionalAssemblies)
    {
        var entryAssembly = Assembly.GetCallingAssembly();

        var hashSet = new HashSet<Assembly>
        {
            entryAssembly,
        };
        foreach (var additionalAssembly in additionalAssemblies)
        {
            hashSet.Add(additionalAssembly);
        }

        b.Scan(
            scan => scan
                .FromAssemblies(hashSet)
                .AddClasses(classes => classes.AssignableTo<IConfig>())
                .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                .AsMatchingInterface()
                .WithSingletonLifetime());
    }
}