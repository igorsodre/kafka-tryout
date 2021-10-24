using System.IO;
using System.Reflection;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tests.Unit.Helpers
{
    public class DatabaseHelper
    {
        private static readonly object Lock = new();

        private static DataContext _context;
        
        public static DataContext GetCleanDatabaseContext()
        {
            lock (Lock)
            {
                if (_context is null)
                {
                    _context = new DataContext(GetDbContextOption());
                }

                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();
            }

            return _context;
        }

        public static DbContextOptions<DataContext> GetDbContextOption()
        {
            return new DbContextOptionsBuilder<DataContext>()
                .UseSqlServer(GetTestDbConnectionString)
                .Options;
        }

        private static string _testDbConnectionString;

        public static string GetTestDbConnectionString
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
