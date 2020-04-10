using System;
using Xunit;
using yaos.OpenApi.Diff.Tests._Base;

namespace yaos.OpenApi.Diff.Tests.Tests
{
    public class PathDiffTest : BaseTest
    {
        private const string OpenAPIPath1 = "path_1.yaml";
        private const string OpenAPIPath2 = "path_2.yaml";
        private const string OpenAPIPath3 = "path_3.yaml";

        [Fact]
        public void TestEqual()
        {
           TestUtils.AssertOpenAPIAreEquals(OpenAPIPath1, OpenAPIPath2);
        }

        [Fact]
        public void TestMultiplePathWithSameSignature()
        {
            Assert.Throws<ArgumentException>(() => TestUtils.AssertOpenAPIAreEquals(OpenAPIPath3, OpenAPIPath3));
        }
}
}
