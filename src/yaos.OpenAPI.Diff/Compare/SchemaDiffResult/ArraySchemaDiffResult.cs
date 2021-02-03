using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using yaos.OpenAPI.Diff.BusinessObjects;
using yaos.OpenAPI.Diff.Enums;
using yaos.OpenAPI.Diff.Extensions;

namespace yaos.OpenAPI.Diff.Compare.SchemaDiffResult
{
    public class ArraySchemaDiffResult : SchemaDiffResult
    {
        public ArraySchemaDiffResult(OpenApiDiff openApiDiff) : base("array", openApiDiff)
        {
        }

        public override ChangedSchemaBO Diff<T>(OpenApiComponents leftComponents, OpenApiComponents rightComponents, T left,
            T right, DiffContextBO context)
        {
            if (left.GetSchemaType() != SchemaTypeEnum.ArraySchema
                || right.GetSchemaType() != SchemaTypeEnum.ArraySchema)
                return null;

            base.Diff(leftComponents, rightComponents, left, right, context);

            var diff = OpenApiDiff
                .SchemaDiff
                .Diff(
                    left.Items,
                    right.Items,
                    context.CopyWithRequired(true));
            if (diff != null)
                ChangedSchema.Items = diff;

            return IsApplicable(context);
        }
    }
}
