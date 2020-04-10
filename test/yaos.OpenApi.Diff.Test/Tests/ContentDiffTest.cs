using openapi_diff;
using Xunit;
using yaos.OpenApi.Diff.Tests._Base;

namespace yaos.OpenAPI.Diff.Tests.Tests
{
    public class ContentDiffTest : BaseTest
    {
        private readonly IOpenAPICompare _openAPICompare;
        private const string OpenAPIDoc1 = "content_diff_1.yaml";
        private const string OpenAPIDoc2 = "content_diff_2.yaml";

        public ContentDiffTest(IOpenAPICompare openAPICompare)
        {
            _openAPICompare = openAPICompare;
        }

        [Fact]
        public void TestContentDiffWithOneEmptyMediaType()
        {
            var changedOpenAPI = _openAPICompare.FromLocations(OpenAPIDoc1, OpenAPIDoc2);
            Assert.True(changedOpenAPI.isIncompatible());
        }

        [Fact]
        public void TestContentDiffWithEmptyMediaTypes()
        {
            var changedOpenAPI = _openAPICompare.FromLocations(OpenAPIDoc1, OpenAPIDoc1);
            Assert.True(changedOpenAPI.isUnchanged());
        }

        [Fact]
        public void TestSameContentDiff()
        {
            var changedOpenAPI = _openAPICompare.FromLocations(OpenAPIDoc2, OpenAPIDoc2);
            Assert.True(changedOpenAPI.isUnchanged());
        }
    }
}
