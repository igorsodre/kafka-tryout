using System;
using System.Linq;
using API.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions;

public static class ConfigurationExtension
{
    public static void SetupConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var installers = typeof(Startup).Assembly.ExportedTypes
            .Where(x =>
                typeof(IServiceInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IServiceInstaller>();

        foreach (var installer in installers)
        {
            installer.InstallServices(services, configuration);
        }
    }
}