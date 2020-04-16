using System;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using openapi_diff.compare;
using System.Collections.Generic;
using openapi_diff.DTOs;
using openapi_diff.Extensions;

namespace openapi_diff.Compare.SchemaDiffResult
{
    public class ArraySchemaDiffResult : SchemaDiffResult
    {
        public ArraySchemaDiffResult(OpenApiDiff openApiDiff) : base("array", openApiDiff)
        {
        }

        public override ChangedSchemaBO Diff<T>(HashSet<string> refSet, OpenApiComponents leftComponents, OpenApiComponents rightComponents, T left,
            T right, DiffContextBO context)
        {
            if (left.GetSchemaType() != SchemaTypeEnum.ArraySchema
                || right.GetSchemaType() != SchemaTypeEnum.ArraySchema)
                return null;

            base.Diff(refSet, leftComponents, rightComponents, left, right, context);

            var diff = OpenApiDiff
                .SchemaDiff
                .Diff(
                    refSet,
                    left.Items,
                    right.Items,
                    context.copyWithRequired(true));
            if (diff != null)
                ChangedSchema.Items = diff;

            return IsApplicable(context);
        }
    }
}
