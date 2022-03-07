using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace DataAccess;

public static class DataAccessLibraryConfig
{
    public static string GetConnectionString()
    {
        var buildDir = Path.GetDirectoryName(
            Assembly.GetExecutingAssembly()
                .Location
        );
        var filePath = Path.Combine(buildDir, "databasesettings.json");
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(filePath)
            .Build()
            .GetConnectionString("SqlServer");
    }
}