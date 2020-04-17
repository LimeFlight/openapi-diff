using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;
using yaos.OpenApi.Diff.Tests._Base;
using yaos.OpenAPI.Diff.Output;

namespace yaos.OpenAPI.Diff.Tests.Tests
{
    public class OpenAPIDiffTest : BaseTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public OpenAPIDiffTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        const string SwaggerV2Http = "http://petstore.swagger.io/v2/swagger.json";
        private const string OpenAPIDoc1 = "Resources\\petstore_v2_1.yaml";
        private const string OpenAPIDoc2 = "Resources\\petstore_v2_2.yaml";
        private const string OpenAPIEmptyDoc = "Resources\\petstore_v2_empty.yaml";
        
        [Fact]
        public void TestEqual()
        {
            TestUtils.AssertOpenAPIAreEquals(OpenAPIDoc2, OpenAPIDoc2);
        }

        [Fact]
        public void TestNewAPI()
        {
            var changedOpenAPI = TestUtils.GetOpenAPICompare().FromLocations(OpenAPIEmptyDoc, OpenAPIDoc2);
            var newEndpoints = changedOpenAPI.NewEndpoints;
            var missingEndpoints = changedOpenAPI.MissingEndpoints;
            var changedEndPoints = changedOpenAPI.ChangedOperations;
            var html =
                new HtmlRender("Changelog", "http://deepoove.com/swagger-diff/stylesheets/demo.css")
                    .Render(changedOpenAPI);

            try
            {
                File.WriteAllText("testNewAPI.html", html);
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine(e.ToString());
            }
            Assert.NotEmpty(newEndpoints);
            Assert.Empty(missingEndpoints);
            Assert.Empty(changedEndPoints);
        }

        [Fact]
        public void TestDeprecatedAPI()
        {
            var changedOpenAPI = TestUtils.GetOpenAPICompare().FromLocations(OpenAPIDoc1, OpenAPIEmptyDoc);
            var newEndpoints = changedOpenAPI.NewEndpoints;
            var missingEndpoints = changedOpenAPI.MissingEndpoints;
            var changedEndPoints = changedOpenAPI.ChangedOperations;
            var html =
                new HtmlRender("Changelog", "http://deepoove.com/swagger-diff/stylesheets/demo.css")
                    .Render(changedOpenAPI);

            try
            {
                File.WriteAllText("testDeprecatedAPI.html", html);
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine(e.ToString());
            }
            Assert.Empty(newEndpoints);
            Assert.NotEmpty(missingEndpoints);
            Assert.Empty(changedEndPoints);
        }

        [Fact]
        public void TestDiff()
        {
            var changedOpenAPI = TestUtils.GetOpenAPICompare().FromLocations(OpenAPIDoc1, OpenAPIDoc2);
            var changedEndPoints = changedOpenAPI.ChangedOperations;
            var html =
                new HtmlRender("Changelog", "http://deepoove.com/swagger-diff/stylesheets/demo.css")
                    .Render(changedOpenAPI);
            try
            {
                File.WriteAllText("testDiff.html", html);
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine(e.ToString());
            }
            Assert.NotEmpty(changedEndPoints);
        }

        [Fact]
        public void TestDiffAndMarkdown()
        {
            var changedOpenAPI = TestUtils.GetOpenAPICompare().FromLocations(OpenAPIDoc1, OpenAPIDoc2);
            var logger = _testOutputHelper.BuildLoggerFor<MarkdownRender>();
            var render = new MarkdownRender(logger).Render(changedOpenAPI);
            try
            {
                File.WriteAllText("testDiff.md", render);

            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine(e.ToString());
            }
        }
    }
}
