using LimeFlight.OpenApi.Diff.Tests._Base;
using Xunit;

namespace LimeFlight.OpenAPI.Diff.Tests.Tests
{
    public class BackwardCompatibilityTest : BaseTest
    {
        private const string OpenAPIDoc1 = "Resources/backwardCompatibility/bc_1.yaml";
        private const string OpenAPIDoc2 = "Resources/backwardCompatibility/bc_2.yaml";
        private const string OpenAPIDoc3 = "Resources/backwardCompatibility/bc_3.yaml";
        private const string OpenAPIDoc4 = "Resources/backwardCompatibility/bc_4.yaml";
        private const string OpenAPIDoc5 = "Resources/backwardCompatibility/bc_5.yaml";
        private const string OpenAPIDoc6 = "Resources/backwardCompatibility/bc_6.yaml";
        private const string OpenAPIDoc7 = "Resources/backwardCompatibility/bc_7.yaml";
        private const string OpenAPIDoc8 = "Resources/backwardCompatibility/bc_8.yaml";

        [Fact]
        public void TestNoChange()
        {
            TestUtils.AssertOpenAPIBackwardCompatible(OpenAPIDoc1, OpenAPIDoc1, false);
        }

        [Fact]
        public void TestAPIAdded()
        {
            TestUtils.AssertOpenAPIBackwardCompatible(OpenAPIDoc1, OpenAPIDoc2, true);
        }

        [Fact]
        public void TestAPIMissing()
        {
            TestUtils.AssertOpenAPIBackwardIncompatible(OpenAPIDoc2, OpenAPIDoc1);
        }

        [Fact]
        public void TestAPIChangedOperationAdded()
        {
            TestUtils.AssertOpenAPIBackwardCompatible(OpenAPIDoc2, OpenAPIDoc3, true);
        }

        [Fact]
        public void TestAPIChangedOperationMissing()
        {
            TestUtils.AssertOpenAPIBackwardIncompatible(OpenAPIDoc3, OpenAPIDoc2);
        }

        [Fact]
        public void TestAPIOperationChanged()
        {
            TestUtils.AssertOpenAPIBackwardCompatible(OpenAPIDoc2, OpenAPIDoc4, true);
        }

        [Fact]
        public void TestAPIReadWriteOnlyPropertiesChanged()
        {
            TestUtils.AssertOpenAPIBackwardCompatible(OpenAPIDoc1, OpenAPIDoc5, true);
        }

        [Fact]
        public void TestAPIMaxLengthPropertyInRequestDecreasedInvalid()
        {
            TestUtils.AssertOpenAPIBackwardIncompatible(OpenAPIDoc5, OpenAPIDoc6);
        }

        [Fact]
        public void TestAPIMaxLengthPropertyInRequestIncreasedValid()
        {
            TestUtils.AssertOpenAPIBackwardCompatible(OpenAPIDoc6, OpenAPIDoc5, true);
        }

        [Fact]
        public void TestAPIMinLengthPropertyInResponseIncreasedInvalid()
        {
            TestUtils.AssertOpenAPIBackwardIncompatible(OpenAPIDoc7, OpenAPIDoc8);
        }

        [Fact]
        public void TestAPIMinLengthPropertyInRequestDecreasedValid()
        {
            TestUtils.AssertOpenAPIBackwardCompatible(OpenAPIDoc8, OpenAPIDoc7, true);
        }
    }
}
