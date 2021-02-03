﻿using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using yaos.OpenAPI.Diff.BusinessObjects;
using yaos.OpenAPI.Diff.Enums;
using yaos.OpenAPI.Diff.Utils;

namespace yaos.OpenAPI.Diff.Compare
{
    public class HeaderDiff : ReferenceDiffCache<OpenApiHeader, ChangedHeaderBO>
    {
        private static readonly RefPointer<OpenApiHeader> RefPointer = new RefPointer<OpenApiHeader>(RefTypeEnum.Headers);
        private readonly OpenApiDiff _openApiDiff;
        private readonly OpenApiComponents _leftComponents;
        private readonly OpenApiComponents _rightComponents;

        public HeaderDiff(OpenApiDiff openApiDiff)
        {
            _openApiDiff = openApiDiff;
            _leftComponents = openApiDiff.OldSpecOpenApi?.Components;
            _rightComponents = openApiDiff.NewSpecOpenApi?.Components;
        }

        public ChangedHeaderBO Diff(OpenApiHeader left, OpenApiHeader right, DiffContextBO context)
        {
            return CachedDiff(left, right, left.Reference?.ReferenceV3, right.Reference?.ReferenceV3, context);
        }

        protected override ChangedHeaderBO ComputeDiff(OpenApiHeader left, OpenApiHeader right, DiffContextBO context)
        {
            left = RefPointer.ResolveRef(_leftComponents, left, left.Reference?.ReferenceV3);
            right = RefPointer.ResolveRef(_rightComponents, right, right.Reference?.ReferenceV3);

            var changedHeader =
                new ChangedHeaderBO(left, right, context)
                {
                    Required = GetBooleanDiff(left.Required, right.Required),
                    Deprecated = !left.Deprecated && right.Deprecated,
                    Style = left.Style != right.Style,
                    Explode = GetBooleanDiff(left.Explode, right.Explode),
                    Description = _openApiDiff
                        .MetadataDiff
                        .Diff(left.Description, right.Description, context),
                    Schema = _openApiDiff
                        .SchemaDiff
                        .Diff(left.Schema, right.Schema, context.CopyWithRequired(true)),
                    Content = _openApiDiff
                        .ContentDiff
                        .Diff(left.Content, right.Content, context),
                    Extensions = _openApiDiff
                        .ExtensionsDiff
                        .Diff(left.Extensions, right.Extensions, context)
                };

            return ChangedUtils.IsChanged(changedHeader);
        }

        private static bool GetBooleanDiff(bool? left, bool? right)
        {
            var leftRequired = left ?? false;
            var rightRequired = right ?? false;
            return leftRequired != rightRequired;
        }
    }
}
