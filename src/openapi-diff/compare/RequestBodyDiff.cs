using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using openapi_diff.Compare;
using openapi_diff.DTOs;
using openapi_diff.utils;
using System.Collections.Generic;
using System.Linq;

namespace openapi_diff.compare
{
    public class RequestBodyDiff : ReferenceDiffCache<OpenApiRequestBody, ChangedRequestBodyBO>
    {
        private static readonly RefPointer<OpenApiRequestBody> RefPointer = new RefPointer<OpenApiRequestBody>(RefTypeEnum.RequestBodies);
        private readonly OpenApiDiff _openApiDiff;

        public RequestBodyDiff(OpenApiDiff openApiDiff)
        {
            this._openApiDiff = openApiDiff;
        }

        private static Dictionary<string, object> GetExtensions(OpenApiRequestBody body)
        {
            return body.Extensions.ToDictionary(x => x.Key, x => (object)x.Value);
        }

        public ChangedRequestBodyBO Diff(
            OpenApiRequestBody left, OpenApiRequestBody right, DiffContextBO context)
        {
            var leftRef = left?.Reference.ReferenceV3;
            var rightRef = right?.Reference.ReferenceV3;
            return CachedDiff(new HashSet<string>(), left, right, leftRef, rightRef, context);
        }

        protected override ChangedRequestBodyBO ComputeDiff(HashSet<string> refSet, OpenApiRequestBody left, OpenApiRequestBody right,
            DiffContextBO context)
        {
            Dictionary<string, OpenApiMediaType> oldRequestContent = null;
            Dictionary<string, OpenApiMediaType> newRequestContent = null;
            OpenApiRequestBody oldRequestBody = null;
            OpenApiRequestBody newRequestBody = null;
            if (left != null)
            {
                oldRequestBody =
                    RefPointer.ResolveRef(
                        _openApiDiff.OldSpecOpenApi.Components, left, left.Reference.ReferenceV3);
                if (oldRequestBody.Content != null)
                {
                    oldRequestContent = (Dictionary<string, OpenApiMediaType>) oldRequestBody.Content;
                }
            }
            if (right != null)
            {
                newRequestBody =
                    RefPointer.ResolveRef(
                        _openApiDiff.NewSpecOpenApi.Components, right, right.Reference.ReferenceV3);
                if (newRequestBody.Content != null)
                {
                    newRequestContent = (Dictionary<string, OpenApiMediaType>) newRequestBody.Content;
                }
            }
            var leftRequired =
                oldRequestBody != null && oldRequestBody.Required;
            var rightRequired =
                newRequestBody != null && newRequestBody.Required;

            var changedRequestBody =
                new ChangedRequestBodyBO(oldRequestBody, newRequestBody, context)
                {
                    ChangeRequired = leftRequired != rightRequired,
                    Description = _openApiDiff
                        .MetadataDiff
                        .diff(
                            oldRequestBody?.Description,
                            newRequestBody?.Description,
                            context),
                    Content = _openApiDiff
                        .ContentDiff
                        .Diff(oldRequestContent, newRequestContent, context),
                    Extensions = _openApiDiff
                        .ExtensionsDiff
                        .diff(GetExtensions(left), GetExtensions(right), context)
                };

            return ChangedUtils.IsChanged(changedRequestBody);
        }
    }
}
