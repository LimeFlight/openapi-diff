using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using openapi_diff.compare;
using System.Collections.Generic;

namespace openapi_diff.Compare.SchemaDiffResult
{
    public class ArraySchemaDiffResult : SchemaDiffResult
    {
        public ArraySchemaDiffResult(OpenApiDiff openApiDiff) : base("array", openApiDiff)
        {
        }

        public override ChangedSchemaBO diff<TV>(HashSet<string> refSet, OpenApiComponents leftComponents, OpenApiComponents rightComponents, TV left,
            TV right, DiffContextBO context)
        {
            var leftArraySchema = (ArraySchema)left;
            var rightArraySchema = (ArraySchema)right;

            base.diff(refSet, leftComponents, rightComponents, left, right, context);

            openApiDiff
                .getSchemaDiff()
                .diff(
                    refSet,
                    leftArraySchema.getItems(),
                    rightArraySchema.getItems(),
                    context.copyWithRequired(true))
                .ifPresent(changedSchema::setItems);
            return isApplicable(context);
        }
    }
}
