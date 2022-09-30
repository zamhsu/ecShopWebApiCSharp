using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTests.TestUtilities;

[CollectionDefinition(nameof(DatabaseCollectionFixture))]
public class DatabaseCollectionFixture : ICollectionFixture<TestDatabaseFixture>
{

}
