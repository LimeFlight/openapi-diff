using Xunit;
using yaos.OpenApi.Diff.Tests._Base;

namespace yaos.OpenApi.Diff.Tests.tests
{
    public class AddPropDiffTest : BaseTest
    {
        private const string OpenAPIDoc1 = "add-prop-1.yaml";
        private const string OpenAPIDoc2 = "add-prop-2.yaml";

        [Fact]
        public void TestDiffSame()
        {
            TestUtils.AssertOpenApiAreEquals(OpenAPIDoc1, OpenAPIDoc2);
        }

        [Fact]
        public void TestDiffDifferent()
        {
            TestUtils.AssertOpenApiBackwardIncompatible(OpenAPIDoc1, OpenAPIDoc2);
        }
    }
}
