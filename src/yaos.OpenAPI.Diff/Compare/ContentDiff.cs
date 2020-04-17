using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using yaos.OpenAPI.Diff.BusinessObjects;
using yaos.OpenAPI.Diff.Utils;

namespace yaos.OpenAPI.Diff.Compare
{
    public class ContentDiff : IEquatable<IDictionary<string, OpenApiMediaType>>
    {
        private readonly OpenApiDiff _openApiDiff;

        public ContentDiff(OpenApiDiff openApiDiff)
        {
            _openApiDiff = openApiDiff;
        }

        public bool Equals(IDictionary<string, OpenApiMediaType> other)
        {
            return false;
        }

        public ChangedContentBO Diff(IDictionary<string, OpenApiMediaType> left, IDictionary<string, OpenApiMediaType> right, DiffContextBO context)
        {
            var leftDict = (Dictionary<string, OpenApiMediaType>)left;
            var rightDict = (Dictionary<string, OpenApiMediaType>)right;


            var mediaTypeDiff = MapKeyDiff<string, OpenApiMediaType>.Diff(leftDict, rightDict);
            var sharedMediaTypes = mediaTypeDiff.SharedKey;
            var changedMediaTypes = new Dictionary<string, ChangedMediaTypeBO>();
            foreach (var sharedMediaType in sharedMediaTypes)
            {
                var oldMediaType = left[sharedMediaType];
                var newMediaType = right[sharedMediaType];
                var changedMediaType =
                    new ChangedMediaTypeBO(oldMediaType?.Schema, newMediaType?.Schema, context)
                    {
                        Schema = _openApiDiff
                            .SchemaDiff
                            .Diff(
                                new HashSet<string>(),
                                oldMediaType?.Schema,
                                newMediaType?.Schema,
                                context.copyWithRequired(true))
                    };
                if (!ChangedUtils.IsUnchanged(changedMediaType))
                {
                    changedMediaTypes.Add(sharedMediaType, changedMediaType);
                }
            }

            return ChangedUtils.IsChanged(new ChangedContentBO(leftDict, rightDict, context)
            {
                Increased = mediaTypeDiff.Increased,
                Missing = mediaTypeDiff.Missing,
                Changed = changedMediaTypes
            });
        }
    }
}
