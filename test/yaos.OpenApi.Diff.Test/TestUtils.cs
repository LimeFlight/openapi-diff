using Xunit;
using yaos.OpenApi.Diff.Tests;

namespace yaos.OpenAPI.Diff.Tests
{
    public class TestUtils : ITestUtils
    {
        private readonly IOpenAPICompare _openAPICompare;

        public TestUtils(IOpenAPICompare openAPICompare)
        {
            _openAPICompare = openAPICompare;
        }

        public void AssertOpenAPIAreEquals(string oldSpec, string newSpec)
        {
            var changedOpenAPI = _openAPICompare.FromLocations(oldSpec, newSpec);
            Assert.Empty(changedOpenAPI.NewEndpoints);
            Assert.Empty(changedOpenAPI.MissingEndpoints);
            Assert.Empty(changedOpenAPI.ChangedOperations);
        }

        public void AssertOpenAPIChangedEndpoints(string oldSpec, string newSpec)
        {
            var changedOpenAPI = _openAPICompare.FromLocations(oldSpec, newSpec);
            Assert.Empty(changedOpenAPI.NewEndpoints);
            Assert.Empty(changedOpenAPI.MissingEndpoints);
            Assert.NotEmpty(changedOpenAPI.ChangedOperations);
        }

        public void AssertOpenAPIBackwardCompatible(string oldSpec, string newSpec, bool isDiff)
        {
            var changedOpenAPI = _openAPICompare.FromLocations(oldSpec, newSpec);
            Assert.True(changedOpenAPI.IsCompatible());
        }

        public void AssertOpenAPIBackwardIncompatible(string oldSpec, string newSpec)
        {
            var changedOpenAPI = _openAPICompare.FromLocations(oldSpec, newSpec);
            Assert.True(changedOpenAPI.IsIncompatible());
        }

        public IOpenAPICompare GetOpenAPICompare()
        {
            return _openAPICompare;
        }
    }
}
