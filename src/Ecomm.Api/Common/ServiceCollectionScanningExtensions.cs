using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Ecomm.Api.Common;

public static class ServiceCollectionScanningExtensions
{
    /// <summary>
    /// Registers every "{Name}" class that implements a matching "I{Name}" interface found in the given
    /// assemblies. Throws at startup if an interface has zero or more than one matching implementation,
    /// instead of silently picking the wrong one.
    /// </summary>
    public static IServiceCollection AddGeneratedServices(
        this IServiceCollection services,
        ServiceLifetime lifetime,
        params Assembly[] assemblies)
    {
        var types = assemblies.SelectMany(a => a.GetTypes()).ToArray();

        var interfaces = types.Where(t =>
            t.IsInterface
            && t.Name.StartsWith('I')
            && (t.Name.EndsWith("Repository", StringComparison.Ordinal) || t.Name.EndsWith("Service", StringComparison.Ordinal)));

        foreach (var @interface in interfaces)
        {
            var expectedImplementationName = @interface.Name[1..];

            var candidates = types
                .Where(t => t.IsClass && !t.IsAbstract
                    && @interface.IsAssignableFrom(t)
                    && t.Name == expectedImplementationName)
                .ToArray();

            if (candidates.Length == 0)
            {
                continue;
            }

            if (candidates.Length > 1)
            {
                throw new InvalidOperationException(
                    $"Found {candidates.Length} implementations of '{@interface.FullName}' named "
                    + $"'{expectedImplementationName}' across {string.Join(", ", candidates.Select(c => c.Assembly.GetName().Name))}. "
                    + "Registration must be explicit when a naming convention is ambiguous.");
            }

            services.Add(new ServiceDescriptor(@interface, candidates[0], lifetime));
        }

        return services;
    }
}
