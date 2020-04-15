using Microsoft.OpenApi.Models;
using openapi_diff.DTOs;
using System.Collections.Generic;

namespace openapi_diff.BusinessObjects
{
    public class ChangedParameterBO : ComposedChangedBO
    {
        private readonly DiffContextBO _context;
        public ParameterLocation? In { get; set; }
        public string Name { get; set; }
        public OpenApiParameter OldParameter { get; set; }
        public OpenApiParameter NewParameter { get; set; }
        public bool IsChangeRequired { get; set; }
        public bool IsDeprecated { get; set; }
        public bool ChangeStyle { get; set; }
        public bool ChangeExplode { get; set; }
        public bool ChangeAllowEmptyValue { get; set; }
        public ChangedMetadataBO Description { get; set; }
        public ChangedSchemaBO Schema { get; set; }
        public ChangedContentBO Content { get; set; }
        public ChangedExtensionsBO Extensions { get; set; }

        public ChangedParameterBO(string name, ParameterLocation? @in, DiffContextBO context)
        {
            _context = context;
            Name = name;
            In = @in;
        }

        public override List<ChangedBO> GetChangedElements()
        {
            return new List<ChangedBO> { Description, Schema, Content, Extensions };
        }

        public override DiffResultBO IsCoreChanged()
        {
            if (!IsChangeRequired
                && !IsDeprecated
                && !ChangeAllowEmptyValue
                && !ChangeStyle
                && !ChangeExplode)
            {
                return new DiffResultBO(DiffResultEnum.NoChanges);
            }
            if ((!IsChangeRequired || OldParameter.Required)
                && (!ChangeAllowEmptyValue || NewParameter.AllowEmptyValue)
                && !ChangeStyle
                && !ChangeExplode)
            {
                return new DiffResultBO(DiffResultEnum.Compatible);
            }
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}
