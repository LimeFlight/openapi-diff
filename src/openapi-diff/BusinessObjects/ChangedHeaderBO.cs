using Microsoft.OpenApi.Models;
using openapi_diff.DTOs;
using System.Collections.Generic;

namespace openapi_diff.BusinessObjects
{
    public class ChangedHeaderBO : ComposedChangedBO
    {
        private readonly OpenApiHeader _oldHeader;
        private readonly OpenApiHeader _newHeader;
        private readonly DiffContextBO _context;

        public bool Required { get; set; }
        public bool Deprecated { get; set; }
        public bool Style { get; set; }
        public bool Explode { get; set; }
        public ChangedMetadataBO Description { get; set; }
        public ChangedSchemaBO Schema { get; set; }
        public ChangedContentBO Content { get; set; }
        public ChangedExtensionsBO Extensions { get; set; }

        public ChangedHeaderBO(OpenApiHeader oldHeader, OpenApiHeader newHeader, DiffContextBO context)
        {
            _oldHeader = oldHeader;
            _newHeader = newHeader;
            _context = context;
        }
        
        public override List<ChangedBO> GetChangedElements()
        {
            return new List<ChangedBO> { Description, Schema, Content, Extensions };
        }

        public override DiffResultBO IsCoreChanged()
        {
            if (!Required && !Deprecated && !Style && !Explode)
            {
                return new DiffResultBO(DiffResultEnum.NoChanges);
            }
            if (!Required && !Style && !Explode)
            {
                return new DiffResultBO(DiffResultEnum.Compatible);
            }
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}
