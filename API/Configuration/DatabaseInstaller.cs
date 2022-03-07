using API.Services;
using API.Services.Interfaces;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Configuration;

public class DatabaseInstaller : IServiceInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options => {
            options.UseSqlServer(DataAccessLibraryConfig.GetConnectionString());
        });


        services.AddScoped<IMessageRepository, MessageRepository>();
    }
}