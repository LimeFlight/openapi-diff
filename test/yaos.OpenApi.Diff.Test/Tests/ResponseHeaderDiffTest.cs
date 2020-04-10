using openapi_diff;
using System.Linq;
using Xunit;
using yaos.OpenApi.Diff.Tests._Base;

namespace yaos.OpenApi.Diff.Tests.Tests
{
    public class ResponseHeaderDiffTest : BaseTest
    {
        private readonly IOpenAPICompare _openAPICompare;
        private const string OPENAPI_DOC1 = "header_1.yaml";
        private const string OPENAPI_DOC2 = "header_2.yaml";

        public ResponseHeaderDiffTest(IOpenAPICompare openAPICompare)
        {
            _openAPICompare = openAPICompare;
        }

        [Fact]
        public void TestDiffDifferent()
        {
            var changedOpenAPI = _openAPICompare.FromLocations(OPENAPI_DOC1, OPENAPI_DOC2);

            Assert.Empty(changedOpenAPI.NewEndpoints);
            Assert.Empty(changedOpenAPI.MissingEndpoints);
            Assert.Empty(changedOpenAPI.ChangedOperations);

            var changedResponses = changedOpenAPI.ChangedOperations.FirstOrDefault()?.APIResponses.Changed;

            Assert.NotNull(changedResponses);
            Assert.NotEmpty(changedResponses);
            Assert.Contains("200", changedResponses);
            
            var changedHeaders = changedResponses["200"].Headers;
            Assert.True(changedHeaders.isDifferent());
            Assert.Equal(changedHeaders.getChanged(), 1);
            Assert.Equal(changedHeaders.getIncreased(), 1);
            Assert.Equal(changedHeaders.getMissing(), 1);
        }
    }
}
