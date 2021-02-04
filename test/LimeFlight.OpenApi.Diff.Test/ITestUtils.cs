using System.Collections.Generic;
using LimeFlight.OpenAPI.Diff;
using LimeFlight.OpenAPI.Diff.BusinessObjects;
using LimeFlight.OpenAPI.Diff.Enums;

namespace LimeFlight.OpenApi.Diff.Tests
{
    public interface ITestUtils
    {
        void AssertOpenAPIAreEquals(string oldSpec, string newSpec);
        void AssertOpenAPIChangedEndpoints(string oldSpec, string newSpec);
        void AssertOpenAPIBackwardCompatible(string oldSpec, string newSpec, bool isDiff);
        void AssertOpenAPIBackwardIncompatible(string oldSpec, string newSpec);
        IOpenAPICompare GetOpenAPICompare();
        IEnumerable<ChangedInfosBO> GetChangesOfType(ChangedOpenApiBO changedOpenAPI, DiffResultEnum changeType);
    }
}
