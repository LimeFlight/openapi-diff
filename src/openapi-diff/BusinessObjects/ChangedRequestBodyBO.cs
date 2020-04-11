using Microsoft.OpenApi.Models;
using openapi_diff.DTOs;
using System.Collections.Generic;

namespace openapi_diff.BusinessObjects
{
    public class ChangedRequestBodyBO : ComposedChangedBO
    {

        private readonly OpenApiRequestBody _oldRequestBody;
        private readonly OpenApiRequestBody _newRequestBody;
        private readonly DiffContextBO _context;

        public bool ChangeRequired { get; set; }
        public ChangedMetadataBO Description { get; set; }
        public ChangedContentBO Content { get; set; }
        public ChangedExtensionsBO Extensions { get; set; }

        public ChangedRequestBodyBO(OpenApiRequestBody oldRequestBody, OpenApiRequestBody newRequestBody, DiffContextBO context)
        {
            _oldRequestBody = oldRequestBody;
            _newRequestBody = newRequestBody;
            _context = context;
        }

        public override List<ChangedBO> GetChangedElements()
        {
            return new List<ChangedBO>
            {
                Description, Content, Extensions
            };
        }

        public override DiffResultBO IsCoreChanged()
        {
            if (!ChangeRequired)
            {
                return new DiffResultBO(DiffResultEnum.NoChanges);
            }
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}
