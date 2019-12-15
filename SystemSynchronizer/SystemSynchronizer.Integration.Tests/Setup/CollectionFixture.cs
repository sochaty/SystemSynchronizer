using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SystemSynchronizer.Integration.Tests.Setup
{
    [CollectionDefinition("api")]
    public class CollectionFixture:ICollectionFixture<TestContext>
    {
    }
}
