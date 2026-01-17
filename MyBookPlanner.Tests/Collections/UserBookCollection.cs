using MyBookPlanner.Tests.Fixtures;
using Xunit;

namespace MyBookPlanner.Tests.Collections
{
    // Defines a shared test collection to reuse the same fixture instance
    // across multiple test classes, avoiding expensive object recreation.
    [CollectionDefinition(nameof(UserBookCollection))]
    public class UserBookCollection : ICollectionFixture<UserBookFixture>
    {
        // This class has no code and is only used to apply the collection definition
    }
}
