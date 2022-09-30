using RepositoryTests.TestUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTests.Implements;

[Collection(nameof(DatabaseCollectionFixture))]
public class EfRepository
{
    [Fact]
    public void TestDB()
    {
        Assert.True(true);
    }
}
