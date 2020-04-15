using Microsoft.OpenApi.Models;
using openapi_diff.DTOs;
using System.Collections.Generic;

namespace openapi_diff.BusinessObjects
{
    public class ChangedResponseBO : ComposedChangedBO
    {
        private readonly OpenApiResponse _oldApiResponse;
        private readonly DiffContextBO _context;
        public OpenApiResponse NewApiResponse { get; }
        public ChangedMetadataBO Description { get; set; }
        public ChangedHeadersBO Headers { get; set; }
        public ChangedContentBO Content { get; set; }
        public ChangedExtensionsBO Extensions { get; set; }

        public ChangedResponseBO(OpenApiResponse oldApiResponse, OpenApiResponse newApiResponse, DiffContextBO context)
        {
            this._oldApiResponse = oldApiResponse;
            this.NewApiResponse = newApiResponse;
            this._context = context;
        }


        public override List<ChangedBO> GetChangedElements()
        {
            return new List<ChangedBO> { Description, Headers, Content, Extensions };

        }

        public override DiffResultBO IsCoreChanged()
        {
            return new DiffResultBO(DiffResultEnum.NoChanges);
        }
    }
}
