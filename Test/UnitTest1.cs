using System;
using Test.TestHelpers;
using Xunit;

namespace Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var options = DatabaseHelper.GetCleanDatabaseContext();
        }
    }
}
