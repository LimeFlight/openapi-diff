using Xunit;
using yaos.OpenApi.Diff.Tests._Base;

namespace yaos.OpenApi.Diff.Tests.Tests
{
    public class ReferenceDiffCacheTest : BaseTest
    {
        private const string OpenAPIDoc1 = "schema_diff_cache_1.yaml";

        [Fact]
        public void TestDiffSame()
        {
           TestUtils.AssertOpenAPIAreEquals(OpenAPIDoc1, OpenAPIDoc1);
        }
    }
}
