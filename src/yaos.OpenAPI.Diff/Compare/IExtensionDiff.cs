using yaos.OpenAPI.Diff.BusinessObjects;
using yaos.OpenAPI.Diff.Enums;

namespace yaos.OpenAPI.Diff.Compare
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
