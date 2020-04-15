using openapi_diff.BusinessObjects;
using openapi_diff.DTOs;

namespace openapi_diff.compare
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
