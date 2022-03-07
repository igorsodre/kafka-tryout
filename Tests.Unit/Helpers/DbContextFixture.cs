using System;
using DataAccess;

namespace Tests.Unit.Helpers;

public class DbContextFixture : IDisposable
{
    public DataContext Context;

    public DbContextFixture()
    {
        Context = DatabaseHelper.GetCleanDatabaseContext();
    }

    public void Dispose() { }
}