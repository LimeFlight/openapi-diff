using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using openapi_diff.Compare;
using openapi_diff.DTOs;
using openapi_diff.utils;
using System.Collections.Generic;

namespace openapi_diff.compare
{
    public class ResponseDiff : ReferenceDiffCache<OpenApiResponse, ChangedResponseBO>
    {
        private static readonly RefPointer<OpenApiResponse> RefPointer = new RefPointer<OpenApiResponse>(RefTypeEnum.Responses);
        private readonly OpenApiDiff _openApiDiff;
        private readonly OpenApiComponents _leftComponents;
        private readonly OpenApiComponents _rightComponents;

        public ResponseDiff(OpenApiDiff openApiDiff)
        {
            this._openApiDiff = openApiDiff;
            _leftComponents = openApiDiff.OldSpecOpenApi?.Components;
            _rightComponents = openApiDiff.NewSpecOpenApi?.Components;
        }

        public ChangedResponseBO Diff(OpenApiResponse left, OpenApiResponse right, DiffContextBO context)
        {
            return CachedDiff(new HashSet<string>(), left, right, left.Reference.ReferenceV3, right.Reference.ReferenceV3, context);
        }

        protected override ChangedResponseBO ComputeDiff(HashSet<string> refSet, OpenApiResponse left, OpenApiResponse right, DiffContextBO context)
        {
            left = RefPointer.ResolveRef(_leftComponents, left, left.Reference.ReferenceV3);
            right = RefPointer.ResolveRef(_rightComponents, right, right.Reference.ReferenceV3);

            var changedResponse = new ChangedResponseBO(left, right, context)
            {
                Description = _openApiDiff
                    .MetadataDiff
                    .diff(left.Description, right.Description, context),
                Content = _openApiDiff
                    .ContentDiff
                    .Diff(left.Content, right.Content, context),
                Headers = _openApiDiff
                    .HeadersDiff
                    .diff(left.Headers, right.Headers, context),
                Extensions = _openApiDiff
                    .ExtensionsDiff
                    .diff(left.Extensions, right.Extensions, context)
            };
           
            return ChangedUtils.IsChanged(changedResponse);
        }
    }
}
