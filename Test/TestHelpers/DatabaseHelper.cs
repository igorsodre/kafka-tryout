using System.IO;
using System.Reflection;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Test.TestHelpers
{
    public class DatabaseHelper
    {
        public static DataContext GetCleanDatabaseContext()
        {
            var context = new DataContext(GetDbContextOption());
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        private static DbContextOptions<DataContext> GetDbContextOption()
        {
            return new DbContextOptionsBuilder<DataContext>()
                .UseSqlServer(GetTestDbConnectionString)
                .Options;
        }

        private static string _testDbConnectionString;

        private static string GetTestDbConnectionString
        {
            get
            {
                if (!string.IsNullOrEmpty(_testDbConnectionString))
                {
                    return _testDbConnectionString;
                }

                var buildDir = Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly()
                        .Location
                );
                var filePath = Path.Combine(buildDir, "databasesettings.json");
                return _testDbConnectionString = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(filePath)
                    .Build()
                    .GetConnectionString("SqlServerTest");
            }
        }
    }
}
