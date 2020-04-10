﻿using Xunit;
using yaos.OpenApi.Diff.Tests._Base;

namespace yaos.OpenApi.Diff.Tests.Tests
{
    public class RequestDiffTest : BaseTest
    {
        private const string OpenAPIDoc1 = "request_diff_1.yaml";
        private const string OpenAPIDoc2 = "request_diff_2.yaml";

        [Fact]
        public void TestDiffDifferent()
        {
           TestUtils.AssertOpenAPIChangedEndpoints(OpenAPIDoc1, OpenAPIDoc2);
        }
    }
}
