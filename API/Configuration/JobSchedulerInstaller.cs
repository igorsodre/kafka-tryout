using Hangfire;
using Hangfire.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace API.Configuration;

public class JobSchedulerInstaller : IServiceInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(options =>
            options.UseRedisStorage(
                ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisServer")), 
                new RedisStorageOptions
                {
                    Prefix = "{JobScheduler}:"
                }
            )
        );
    }
}