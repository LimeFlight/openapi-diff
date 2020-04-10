using Xunit;
using yaos.OpenApi.Diff.Tests._Base;

namespace yaos.OpenAPI.Diff.Tests.Tests
{
    public class OneOfDiffTest : BaseTest
    {
        private const string OpenAPIDoc1 = "oneOf_diff_1.yaml";
        private const string OpenAPIDoc2 = "oneOf_diff_2.yaml";
        private const string OpenAPIDoc3 = "oneOf_diff_3.yaml";
        private const string OpenAPIDoc4 = "composed_schema_1.yaml";
        private const string OpenAPIDoc5 = "composed_schema_2.yaml";
        private const string OpenAPIDoc6 = "oneOf_discriminator-changed_1.yaml";
        private const string OpenAPIDoc7 = "oneOf_discriminator-changed_2.yaml";

        [Fact]
        public void TestDiffSame()
        {
            TestUtils.AssertOpenAPIAreEquals(OpenAPIDoc1, OpenAPIDoc1);
        }

        [Fact]
        public void TestDiffDifferentMapping()
        {
            TestUtils.AssertOpenAPIChangedEndpoints(OpenAPIDoc1, OpenAPIDoc2);
        }

        [Fact]
        public void testDiffSameWithOneOf()
        {
            TestUtils.AssertOpenAPIAreEquals(OpenAPIDoc2, OpenAPIDoc3);
        }

        [Fact]
        public void TestComposedSchema()
        {
            TestUtils.AssertOpenAPIBackwardIncompatible(OpenAPIDoc4, OpenAPIDoc5);
        }

        [Fact]
        public void TestOneOfDiscriminatorChanged()
        {
            // The oneOf 'discriminator' changed: 'realtype' -> 'othertype':
            TestUtils.AssertOpenAPIBackwardIncompatible(OpenAPIDoc6, OpenAPIDoc7);
        }
    }
}
