using System.Collections.Generic;
using LimeFlight.OpenAPI.Diff.BusinessObjects;
using LimeFlight.OpenAPI.Diff.Enums;
using LimeFlight.OpenAPI.Diff.Utils;
using Microsoft.OpenApi.Models;

namespace LimeFlight.OpenAPI.Diff.Compare
{
    public class ResponseDiff : ReferenceDiffCache<OpenApiResponse, ChangedResponseBO>
    {
        private static readonly RefPointer<OpenApiResponse> RefPointer =
            new RefPointer<OpenApiResponse>(RefTypeEnum.Responses);

        private readonly OpenApiComponents _leftComponents;
        private readonly OpenApiDiff _openApiDiff;
        private readonly OpenApiComponents _rightComponents;

        public ResponseDiff(OpenApiDiff openApiDiff)
        {
            _openApiDiff = openApiDiff;
            _leftComponents = openApiDiff.OldSpecOpenApi?.Components;
            _rightComponents = openApiDiff.NewSpecOpenApi?.Components;
        }

        public ChangedResponseBO Diff(OpenApiResponse left, OpenApiResponse right, DiffContextBO context)
        {
            return CachedDiff(left, right, left.Reference?.ReferenceV3,
                right.Reference?.ReferenceV3, context);
        }

        protected override ChangedResponseBO ComputeDiff(OpenApiResponse left,
            OpenApiResponse right, DiffContextBO context)
        {
            left = RefPointer.ResolveRef(_leftComponents, left, left.Reference?.ReferenceV3);
            right = RefPointer.ResolveRef(_rightComponents, right, right.Reference?.ReferenceV3);

            var changedResponse = new ChangedResponseBO(left, right, context)
            {
                Description = _openApiDiff
                    .MetadataDiff
                    .Diff(left.Description, right.Description, context),
                Content = _openApiDiff
                    .ContentDiff
                    .Diff(left.Content, right.Content, context),
                Headers = _openApiDiff
                    .HeadersDiff
                    .Diff(left.Headers, right.Headers, context),
                Extensions = _openApiDiff
                    .ExtensionsDiff
                    .Diff(left.Extensions, right.Extensions, context)
            };

            return ChangedUtils.IsChanged(changedResponse);
        }
    }
}