using LimeFlight.OpenAPI.Diff.BusinessObjects;
using LimeFlight.OpenAPI.Diff.Enums;

namespace LimeFlight.OpenAPI.Diff.Compare
{
    public interface IExtensionDiff
    {
        ExtensionDiff SetOpenApiDiff(OpenApiDiff openApiDiff);

        string GetName();

        ChangedBO Diff<T>(ChangeBO<T> extension, DiffContextBO context)
            where T : class;

        bool IsParentApplicable(TypeEnum type, object objectElement, object extension, DiffContextBO context);
    }
}