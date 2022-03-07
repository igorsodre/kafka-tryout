using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Configuration;

public interface IServiceInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration);
}