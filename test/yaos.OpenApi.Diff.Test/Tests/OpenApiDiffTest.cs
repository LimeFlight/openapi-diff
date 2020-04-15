using openapi_diff;
using openapi_diff.output;
using System;
using System.Collections.Generic;
using System.IO;
using openapi_diff.DTOs;
using Xunit;
using yaos.OpenApi.Diff.Tests._Base;

namespace yaos.OpenAPI.Diff.Tests.Tests
{
    public class OpenAPIDiffTest : BaseTest
    {
        const string SwaggerV2Http = "http://petstore.swagger.io/v2/swagger.json";
        private const string OpenAPIDoc1 = "petstore_v2_1.yaml";
        private const string OpenAPIDoc2 = "petstore_v2_2.yaml";
        private const string OpenAPIEmptyDoc = "petstore_v2_empty.yaml";
        
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
            //string html =
            //    new HtmlRender("Changelog", "http://deepoove.com/swagger-diff/stylesheets/demo.css")
            //        .Render(changedOpenAPI);
            string html = "";

            try
            {
                File.WriteAllText("target/testNewAPI.html", html);
            }
            catch (IOException e)
            {
                OutputHelper.WriteLine(e.StackTrace);
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
            //var html =
            //    new HtmlRender("Changelog", "http://deepoove.com/swagger-diff/stylesheets/demo.css")
            //        .Render(changedOpenAPI);
            var html = "";

            try
            {
                File.WriteAllText("target/testDeprecatedAPI.html", html);
            }
            catch (IOException e)
            {
                OutputHelper.WriteLine(e.StackTrace);
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
            //var html =
            //    new HtmlRender("Changelog", "http://deepoove.com/swagger-diff/stylesheets/demo.css")
            //        .Render(changedOpenAPI);
            var html = "";
            try
            {
                File.WriteAllText("target/testDiff.html", html);
            }
            catch (IOException e)
            {
                OutputHelper.WriteLine(e.StackTrace);
            }
            Assert.NotEmpty(changedEndPoints);
        }

        [Fact]
        public void TestDiffAndMarkdown()
        {
            var changedOpenAPI = TestUtils.GetOpenAPICompare().FromLocations(OpenAPIDoc1, OpenAPIDoc2);
            var render = new MarkdownRender().Render(changedOpenAPI);
            try
            {
                File.WriteAllText("target/testDiff.md", render);

            }
            catch (IOException e)
            {
                OutputHelper.WriteLine(e.StackTrace);
            }
        }
    }
}
