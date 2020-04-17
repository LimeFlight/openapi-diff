using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using yaos.OpenAPI.Diff.Enums;
using yaos.OpenAPI.Diff.Extensions;

namespace yaos.OpenAPI.Diff.BusinessObjects
{
    public class ChangedSchemaBO : ComposedChangedBO
    {
        public DiffContextBO Context { get; set; }
        public OpenApiSchema OldSchema { get; set; }
        public OpenApiSchema NewSchema { get; set; }
        public string Type { get; set; }
        public Dictionary<string, ChangedSchemaBO> ChangedProperties { get; set; }
        public Dictionary<string, OpenApiSchema> IncreasedProperties { get; set; }
        public Dictionary<string, OpenApiSchema> MissingProperties { get; set; }
        public bool IsChangeDeprecated { get; set; }
        public ChangedMetadataBO Description { get; set; }
        public bool IsChangeTitle { get; set; }
        public ChangedRequiredBO Required { get; set; }
        public bool IsChangeDefault { get; set; }
        public ChangedEnumBO Enumeration { get; set; }
        public bool IsChangeFormat { get; set; }
        public ChangedReadOnlyBO ReadOnly { get; set; }
        public ChangedWriteOnlyBO WriteOnly { get; set; }
        public bool IsChangedType { get; set; }
        public ChangedMaxLengthBO MaxLength { get; set; }
        public bool DiscriminatorPropertyChanged { get; set; }
        public ChangedSchemaBO Items { get; set; }
        public ChangedOneOfSchemaBO OneOfSchema { get; set; }
        public ChangedSchemaBO AddProp { get; set; }
        public ChangedExtensionsBO Extensions { get; set; }

        public ChangedSchemaBO()
        {
            IncreasedProperties = new Dictionary<string, OpenApiSchema>();
            MissingProperties = new Dictionary<string, OpenApiSchema>();
            ChangedProperties = new Dictionary<string, ChangedSchemaBO>();
        }

        public override List<ChangedBO> GetChangedElements()
        {
            var list = new List<ChangedBO>
            {
                Description,
                ReadOnly,
                WriteOnly,
                Items,
                OneOfSchema,
                AddProp,
                Enumeration,
                Required,
                MaxLength,
                Extensions
            };

            list.AddRange(ChangedProperties.Values);
            return list;
        }

        public override DiffResultBO IsCoreChanged()
        {
            if (
                !IsChangedType
                && (OldSchema == null && NewSchema == null || OldSchema != null && NewSchema != null)
                && !IsChangeFormat
                && IncreasedProperties.Count == 0
                && MissingProperties.Count == 0
                && ChangedProperties.Values.Count == 0
                && !IsChangeDeprecated
                && ! DiscriminatorPropertyChanged
            )
                return new DiffResultBO(DiffResultEnum.NoChanges);

            var compatibleForRequest = OldSchema != null || NewSchema == null;
            var compatibleForResponse =
                MissingProperties.IsNullOrEmpty() && (OldSchema == null || NewSchema != null);

            if ((Context.IsRequest && compatibleForRequest
                 || Context.IsResponse && compatibleForResponse)
                && !IsChangedType
                && !DiscriminatorPropertyChanged)
            {
                return new DiffResultBO(DiffResultEnum.Compatible);
            }
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}
