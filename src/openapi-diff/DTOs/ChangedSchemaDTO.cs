using System.Collections.Generic;
using Microsoft.OpenApi.Models;

namespace openapi_diff.DTOs
{
    public class ChangedSchemaDTO
    {
        protected DiffContextDTO Context { get; set; }
        protected OpenApiSchema OldSchema { get; set; }
        protected OpenApiSchema NewSchema { get; set; }
        protected string Type { get; set; }
        protected Dictionary<string, ChangedSchemaDTO> ChangedProperties { get; set; }
        protected Dictionary<string, OpenApiSchema> IncreasedProperties { get; set; }
        protected Dictionary<string, OpenApiSchema> MissingProperties { get; set; }
        protected bool ChangeDeprecated { get; set; }
        protected ChangedMetadataDTO Description { get; set; }
        protected bool ChangeTitle { get; set; }
        protected ChangedRequiredDTO Required { get; set; }
        protected bool ChangeDefault { get; set; }
        protected ChangedEnumDTO Enumeration { get; set; }
        protected bool ChangeFormat { get; set; }
        protected ChangedReadOnlyDTO ReadOnly { get; set; }
        protected ChangedWriteOnlyDTO WriteOnly { get; set; }
        protected bool ChangedType { get; set; }
        protected ChangedMaxLengthDTO MaxLength { get; set; }
        protected bool DiscriminatorPropertyChanged { get; set; }
        protected ChangedSchemaDTO Items { get; set; }
        protected ChangedOneOfSchemaDTO OneOfSchema { get; set; }
        protected ChangedSchemaDTO AddProp { get; set; }
        private ChangedExtensionsDTO Extensions { get; set; }
    }
}
