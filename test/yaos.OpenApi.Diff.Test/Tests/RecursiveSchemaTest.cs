using Xunit;
using yaos.OpenApi.Diff.Tests._Base;

namespace yaos.OpenApi.Diff.Tests.Tests
{
    public class RecursiveSchemaTest : BaseTest
    {
        private const string OpenAPIDoc1 = "Resources\\recursive_model_1.yaml";
        private const string OpenAPIDoc2 = "Resources\\recursive_model_2.yaml";

        [Fact]
        public void TestDiffSame()
        { 
           TestUtils.AssertOpenAPIAreEquals(OpenAPIDoc1, OpenAPIDoc1);
        }

        [Fact]
        public void TestDiffDifferent()
        {
            TestUtils.AssertOpenAPIBackwardIncompatible(OpenAPIDoc1, OpenAPIDoc2);
        }
    }
}
