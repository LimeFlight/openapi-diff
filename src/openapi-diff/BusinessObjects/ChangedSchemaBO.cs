using Microsoft.OpenApi.Models;
using openapi_diff.DTOs;
using openapi_diff.Extensions;
using System;
using System.Collections.Generic;

namespace openapi_diff.BusinessObjects
{
    public class ChangedSchemaBO : ComposedChangedBO
    {
        protected DiffContextBO Context { get; set; }
        protected OpenApiSchema OldSchema { get; set; }
        protected OpenApiSchema NewSchema { get; set; }
        protected string Type { get; set; }
        protected Dictionary<string, ChangedSchemaBO> ChangedProperties { get; set; }
        protected Dictionary<string, OpenApiSchema> IncreasedProperties { get; set; }
        protected Dictionary<string, OpenApiSchema> MissingProperties { get; set; }
        protected bool ChangeDeprecated { get; set; }
        protected ChangedMetadataBO Description { get; set; }
        protected bool ChangeTitle { get; set; }
        protected ChangedRequiredBO Required { get; set; }
        protected bool ChangeDefault { get; set; }
        protected ChangedEnumBO Enumeration { get; set; }
        protected bool ChangeFormat { get; set; }
        protected ChangedReadOnlyBO ReadOnly { get; set; }
        protected ChangedWriteOnlyBO WriteOnly { get; set; }
        protected bool ChangedType { get; set; }
        protected ChangedMaxLengthBO MaxLength { get; set; }
        protected bool DiscriminatorPropertyChanged { get; set; }
        protected ChangedSchemaBO Items { get; set; }
        protected ChangedOneOfSchemaBO OneOfSchema { get; set; }
        protected ChangedSchemaBO AddProp { get; set; }
        private ChangedExtensionsBO Extensions { get; set; }

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
                !ChangedType
                && (OldSchema == null && NewSchema == null || OldSchema != null && NewSchema != null)
                && !ChangeFormat
                && IncreasedProperties.Count == 0
                && MissingProperties.Count == 0
                && ChangedProperties.Values.Count == 0
                && !ChangeDeprecated
                && ! DiscriminatorPropertyChanged
            )
                return new DiffResultBO(DiffResultEnum.NoChanges);

            var compatibleForRequest = OldSchema != null || NewSchema == null;
            var compatibleForResponse =
                MissingProperties.IsNullOrEmpty() && (OldSchema == null || NewSchema != null);

            if ((Context.IsRequest && compatibleForRequest
                 || Context.IsResponse && compatibleForResponse)
                && !ChangedType
                && !DiscriminatorPropertyChanged)
            {
                return new DiffResultBO(DiffResultEnum.Compatible);
            }
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}
