using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using openapi_diff.compare;
using System.Collections.Generic;
using System.Linq;

namespace openapi_diff.Compare.SchemaDiffResult
{
    public class SchemaDiffResult
    {
        protected ChangedSchemaBO changedSchema;
        protected OpenApiDiff openApiDiff;

        public SchemaDiffResult(OpenApiDiff openApiDiff)
        {
            this.openApiDiff = openApiDiff;
            this.changedSchema = new ChangedSchemaBO();
        }

        public SchemaDiffResult(string type, OpenApiDiff openApiDiff) : this(openApiDiff)
        {
            this.changedSchema.Type = type;
        }

        public virtual ChangedSchemaBO diff<TV>(
          HashSet<string> refSet,
          OpenApiComponents leftComponents,
          OpenApiComponents rightComponents,
          TV left,
          TV right,
          DiffContextBO context)
            where TV : OpenApiSchema
        {
            var changedEnum =
                ListDiff.diff<ChangedEnumBO, object>(new ChangedEnumBO(new List<object> {left.Enum}, new List<object> {right.Enum}, context));

            changedSchema.Context = context;
            changedSchema.OldSchema = left;
            changedSchema.NewSchema = right;
            changedSchema.ChangeDeprecated = !left.Deprecated && right.Deprecated;
            changedSchema.ChangeTitle = left.Title != right.Title;
            changedSchema.Required = ListDiff.diff<ChangedRequiredBO, string>(new ChangedRequiredBO(left.Required.ToList(), right.Required.ToList(), context));
            changedSchema.ChangeDefault = !left.Default.Equals(right.Default);
            changedSchema.Enumeration = changedEnum;
            changedSchema.ChangeFormat = left.Format != right.Format;
            changedSchema.ReadOnly = new ChangedReadOnlyBO(left.ReadOnly, right.ReadOnly, context);
            changedSchema.WriteOnly = new ChangedWriteOnlyBO(left.WriteOnly, right.WriteOnly, context);
            changedSchema.MaxLength = new ChangedMaxLengthBO(left.MaxLength, right.MaxLength, context);

            openApiDiff.
                .getExtensionsDiff()
                .diff(left.getExtensions(), right.getExtensions(), context)
                .ifPresent(changedSchema::setExtensions);
            openApiDiff
                .getMetadataDiff()
                .diff(left.getDescription(), right.getDescription(), context)
                .ifPresent(changedSchema::setDescription);

            var leftProperties = left.Properties;
            var rightProperties = right.Properties;
            var propertyDiff = MapKeyDiff.Diff(leftProperties, rightProperties);

            for (string key : propertyDiff.getSharedKey())
            {
                openApiDiff
                    .getSchemaDiff()
                    .diff(
                        refSet,
                        leftProperties.get(key),
                        rightProperties.get(key),
                        required(context, key, right.getRequired()))
                    .ifPresent(
                        changedSchema1->changedSchema.getChangedProperties().put(key, changedSchema1));
            }

            compareAdditionalProperties(refSet, left, right, context);

            changedSchema
                .getIncreasedProperties()
                .putAll(filterProperties(Change.Type.ADDED, propertyDiff.getIncreased(), context));
            changedSchema
                .getMissingProperties()
                .putAll(filterProperties(Change.Type.REMOVED, propertyDiff.getMissing(), context));
            return isApplicable(context);
        }
    }
}
